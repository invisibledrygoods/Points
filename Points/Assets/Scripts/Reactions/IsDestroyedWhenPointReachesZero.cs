using UnityEngine;
using Require;

public class IsDestroyedWhenPointReachesZero : MonoBehaviour
{
    public string type;
    public Transform spawnAfterDestroying;

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

            if (spawnAfterDestroying != null)
            {
                Instantiate(spawnAfterDestroying, transform.position, Quaternion.identity);
            }
        });
    }

    void LateUpdate()
    {
        waitForLateUpdate.Happened();
    }
}
