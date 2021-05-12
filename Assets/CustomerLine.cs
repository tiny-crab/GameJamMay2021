using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UniRx;

public class CustomerLine : MonoBehaviour
{
    public GameObject orderPrefab;
    public GameObject customerPrefab;

    public UniRx.IObservable<long> clickStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0));

    public Datastore datastore;

    // Start is called before the first frame update
    void Start() {
        orderPrefab = Resources.Load<GameObject>("Prefabs/Order");
        customerPrefab = Resources.Load<GameObject>("Prefabs/Customer");

        clickStream.Subscribe(_ => {
            RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, 1 << 6);
            // this is a really bad way to detect if we hit an order button
            Debug.Log("yay click");
            if (rayHit.collider != null && rayHit.collider.GetComponentInParent<CustomerLine>()) {
                fulfillOrder();
            }
        });

        for (var i = 0; i < 5; i++) {
            generateCustomer();
        }
    }

    void generateCustomer() {
        var origin = this.transform.position;
        var lineOffset = datastore.pendingOrders.Count * 1.5f;
        var customer = GameObject.Instantiate(customerPrefab, origin + new Vector3(0, -lineOffset, 0), Quaternion.identity, this.transform);
        var order = GameObject.Instantiate(orderPrefab, origin + new Vector3(1, -lineOffset, 0), Quaternion.identity, this.transform);
        datastore.pendingOrders.Add(new Datastore.Order() {
            customer = customer,
            orderItems = new List<System.Tuple<GameObject, CropType>>() {
                new System.Tuple<GameObject, CropType>(order, CropTemplates.Potato)
            }
        });
    }

    void fulfillOrder() {
        Debug.Log("Hit an order button yay");
    }

    // Update is called once per frame
    void Update() {

    }
}
