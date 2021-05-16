using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UniRx;
using Unity.Linq;
using DG.Tweening;

public class CustomerLine : MonoBehaviour
{
    public GameObject orderPrefab;
    public GameObject customerPrefab;

    public UniRx.IObservable<long> clickStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0));

    public Datastore datastore;
    public List<Vector3> linePositions = new List<Vector3>();
    public Vector3 finalDestination;

    // Start is called before the first frame update
    void Start() {
        datastore = GameObject.Find("Datastore").GetComponent<Datastore>();
        orderPrefab = datastore.prefabManager.orderPrefab;
        customerPrefab = datastore.prefabManager.customerPrefab;

        clickStream.Subscribe(_ => {
            if (datastore.mouseState.Value == (int) MouseState.DEFAULT) {
                RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity);
                // this is a really gross way to detect if we hit an order button 🤮
                if (rayHit.collider != null && rayHit.collider.GetComponentInParent<CustomerLine>() != null) {
                    clickOrder(rayHit.collider.gameObject);
                }
            }
        });

        linePositions = this.gameObject.Children().Where(child => child.name.StartsWith("Slot")).Select(child => child.transform.position).ToList();
        finalDestination = this.transform.Find("FinalDestination").position;

        linePositions.ForEach(position => generateCustomer(position));

        datastore.turnCount.Subscribe(value => {
            if (value % 12 == 0 && value != 0) {
                datastore.possibleCrops.Add(CropTemplates.cropTypes.Except(datastore.possibleCrops).getRandomElement());
                datastore.newCropNotifier.Value++;
                datastore.tillCount.Value += 3;
            }
            endTurn();
        });
    }

    void generateCustomer(Vector3 linePosition) {
        var origin = this.transform.position;
        var lineOffset = datastore.customers.Count * 1.5f;
        var customer = GameObject.Instantiate(customerPrefab, linePosition, Quaternion.identity, this.transform);
        var animator = customer.GetComponent<Animator>();
        animator.Play("Customer_waiting");

        // this is a very legit way to weight certain values in a random number generator, get on my level
        var numOrders = new List<int>() {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 3}.getRandomElement();
        var orderList = new List<Datastore.Order>();
        for (var i = 0; i < numOrders; i++) {
            var order = GameObject.Instantiate(orderPrefab, linePosition + new Vector3(1 + .75f * i, 0, 0), Quaternion.identity, this.transform);

            var randomCropType = datastore.possibleCrops.getRandomElement();
            order.Children().First().assignSpriteFromPath(randomCropType.getSpritePath(randomCropType.spritePathCount));

            orderList.Add(new Datastore.Order() {
                orderButton = order,
                crop = randomCropType,
                turnsWillingToWait = 3
            });
        }

        datastore.customers.Add(new KeyValuePair<GameObject, List<Datastore.Order>>(customer, orderList));
    }

    void clickOrder(GameObject orderButton) {
        var orderMatch = datastore.customers.First().Value.FindAll(obj => obj.orderButton == orderButton);
        if (orderMatch.Count == 1 && datastore.storage[orderMatch[0].crop].Value > 0) {
            fulfillOrder(orderMatch.Single());
        }
        var animator = datastore.customers.First().Key.GetComponent<Animator>();
        animator.Play("Customer_walking");
    }

    void fulfillOrder(Datastore.Order order) {
        datastore.storage[order.crop].Value -= 1;
        order.orderButton.Children().First().assignSpriteFromPath("UISprites/confirm");
        order.completed = true;
        datastore.ordersFulfilled.Value++;
    }

    void endTurn() {
        bool allOrdersCompleted = datastore.customers.First().Value.All(order => order.completed);
        bool waitedTooLong = datastore.customers.First().Value.First().turnsWillingToWait <= 0;
        if (allOrdersCompleted || waitedTooLong) {
            frontOfLineLeave();
            if (allOrdersCompleted) datastore.customersSatisfied.Value++;
            else if (waitedTooLong) datastore.customersFrustrated.Value++;
        } else {
            datastore.customers.First().Value.First().turnsWillingToWait--;
        }
    }

    void frontOfLineLeave() {
        var customer = datastore.customers.First();
        customer.Value.ForEach(order => GameObject.Destroy(order.orderButton));

        Tween customerTween = DOTween.To(
                ()=> customer.Key.transform.position,
                x=> customer.Key.transform.position = x,
                finalDestination,
                1f);
        customerTween.OnComplete(() => {
            GameObject.Destroy(customer.Key);
        });
        customerTween.Play();
        datastore.customers.Remove(datastore.customers.First());
        shiftCustomers();
        generateCustomer(linePositions.Last());
    }

    void shiftCustomers() {
        for (var i = 0; i < datastore.customers.Count; i++) {
            var customer = datastore.customers[i].Key;
            Tween customerTween = DOTween.To(
                ()=> customer.transform.position,
                x=> customer.transform.position = x,
                linePositions[i],
                .5f).SetEase(Ease.OutQuart);
            customerTween.Play();
            for (var j = 0; j < datastore.customers[i].Value.Count; j++) {
                var orderButton = datastore.customers[i].Value[j].orderButton.transform;
                Tween orderTween = DOTween.To(
                    ()=> orderButton.position,
                    x=> orderButton.position = x,
                    linePositions[i] + new Vector3(1 + .75f * j, 0, 0),
                    .5f).SetEase(Ease.OutExpo).SetDelay(.05f * (j+1));
                orderTween.Play();
            }
        }
    }
}