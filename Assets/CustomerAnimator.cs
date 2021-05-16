using UnityEngine;
using UniRx;

public class CustomerAnimator : MonoBehaviour
{
    public enum CustomerState {
        WAITING,
        SHIFTING,
        LEAVING_HAPPY,
        LEAVING_UNHAPPY
    }

    public Animator animator;
    public ReactiveProperty<CustomerState> state = new ReactiveProperty<CustomerState>(CustomerState.WAITING);
}
