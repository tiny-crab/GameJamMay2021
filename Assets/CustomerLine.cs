using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UniRx;
using Unity.Linq;

public class CustomerLine : MonoBehaviour
{
    public GameObject orderPrefab;
    public GameObject customerPrefab;

    public UniRx.IObservable<long> clickStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0));

    public Datastore datastore;

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

        for (var i = 0; i < 3; i++) {
            generateCustomer();
        }

        datastore.turnCount.Subscribe(_ => endTurn());
    }

    void generateCustomer() {
        var origin = this.transform.position;
        var lineOffset = datastore.customers.Count * 1.5f;
        var customer = GameObject.Instantiate(customerPrefab, origin + new Vector3(0, -lineOffset, 0), Quaternion.identity, this.transform);

        // this is a very legit way to weight certain values in a random number generator, get on my level
        var numOrders = new List<int>() {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 3}.getRandomElement();
        var orderList = new List<Datastore.Order>();
        for (var i = 0; i < numOrders; i++) {
            var order = GameObject.Instantiate(orderPrefab, origin + new Vector3(1 + .75f * i, -lineOffset, 0), Quaternion.identity, this.transform);

            var randomCropType = CropTemplates.cropTypes.getRandomElement();
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
    }

    void fulfillOrder(Datastore.Order order) {
        datastore.storage[order.crop].Value -= 1;
        order.orderButton.Children().First().assignSpriteFromPath("UISprites/confirm");
        order.completed = true;
        //datastore.money.Value += order.crop.sellPrice;
    }

    void endTurn() {
        bool allOrdersCompleted = datastore.customers.First().Value.All(order => order.completed);
        bool waitedTooLong = datastore.customers.First().Value.First().turnsWillingToWait <= 0;
        if (allOrdersCompleted || waitedTooLong) {
            datastore.customers.First().Value.ForEach(order => GameObject.Destroy(order.orderButton));
            GameObject.Destroy(datastore.customers.First().Key);
            datastore.customers.RemoveAt(0);
            shiftCustomers();
            generateCustomer();
        } else {
            datastore.customers.First().Value.First().turnsWillingToWait--;
        }
    }

    void shiftCustomers() {
        for (var i = 0; i < datastore.customers.Count; i++) {
            datastore.customers[i].Key.transform.position = this.transform.position + new Vector3(0, -i, 0);
            for (var j = 0; j < datastore.customers[i].Value.Count; j++) {
                datastore.customers[i].Value[j].orderButton.transform.position = this.transform.position + new Vector3(1 + .75f * j, -i, 0);
            }
        }
    }
}