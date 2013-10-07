using UnityEngine;
using System.Collections;
using Require;
using Shouldly;

public class ReceivesPointsInCollisionTest : TestBehaviour
{
    GameObject butthead;

    public override void Spec()
    {
        Given("it receives 3 'healing' in collisions with things that take 'damage'")
            .And("it has 5 hp")
            .And("a butthead who takes damage is nearby")
            .When("it collides with the butthead")
            .ThenWithin("3 frames", "it should have 8 hp")
            .Because("it should receive points in collisions");
    }

    public void ItReceives____InCollisionsWithThingsThatTake__(float amount, string source, string ifColliderCanReceive)
    {
        ReceivesPointsInCollision receiver = transform.Require<ReceivesPointsInCollision>();
        transform.Require<BoxCollider>().isTrigger = true;
        transform.Require<Rigidbody>().isKinematic = true;
        receiver.amount = amount;
        receiver.source = source;
        receiver.ifColliderCanReceive = ifColliderCanReceive;
    }

    public void ItHas__Hp(float hp)
    {
        HasPoints points = transform.Require<HasPoints>();
        points.Set("hp", hp);
        points.SetModifier("hp", "healing", 1);
    }

    public void AButtheadWhoTakesDamageIsNearby()
    {
        butthead = new GameObject();
        butthead.transform.Require<BoxCollider>().isTrigger = true;
        butthead.transform.Require<Rigidbody>().isKinematic = true;
        butthead.transform.position = transform.position + Vector3.left * 5;

        HasPoints buttheadPoints = butthead.transform.Require<HasPoints>();
        buttheadPoints.Set("hp", 5);
        buttheadPoints.SetModifier("hp", "damage", -1);
    }

    public void ItCollidesWithTheButthead()
    {
        butthead.transform.position = transform.position;
    }

    public void ItShouldHave__Hp(float amount)
    {
        transform.Require<HasPoints>().Get("hp").ShouldBe(amount);
    }
}
