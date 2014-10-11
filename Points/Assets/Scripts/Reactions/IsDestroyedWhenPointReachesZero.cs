using UnityEngine;
using Require;

public class IsDestroyedWhenPointReachesZero : MonoBehaviour
{
    public string type;

    Transform module;
    ReceivesPointEvents pointEvents;

    WaitFor waitForLateUpdate = new WaitFor();

    void Awake()
    {
        module = transform.GetModuleRoot();
        pointEvents = module.Require<ReceivesPointEvents>();
    }

    void Start()
    {
        pointEvents.WaitFor(type).toReachZero.Then(() =>
        {
            waitForLateUpdate.Then(() =>
            {
                Destroy(module.gameObject);
            });
        });
    }

    void LateUpdate()
    {
        waitForLateUpdate.Happened();
    }
}
