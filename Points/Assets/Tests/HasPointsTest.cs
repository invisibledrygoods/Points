using UnityEngine;
using System.Collections;
using Shouldly;
using Require;
using System;

public class HasPointsTest : TestBehaviour
{
    HasPoints it;

    public override void Spec()
    {
        Given("it has 5 'hp'")
            .And("it receives -1 'hp' from 'damage'")
            .When("it receives 3 points of 'damage'")
            .Then("it should have 2 'hp'")
            .Because("it should lose points when a negative modifier is applied");

        Given("it has 5 'hp'")
            .And("it has 5 'shield'")
            .And("it receives -1 'hp' from 'damage'")
            .And("it receives -1 'shield' from 'damage'")
            .When("it receives 3 points of 'damage'")
            .Then("it should have 2 'hp'")
            .And("it should have 2 'shield'")
            .Because("multiple points can be modified by a single source");

        Given("it has 5 'hp'")
            .And("it receives -1 'hp' from 'damage'")
            .And("it receives -1 'hp' from 'damage'")
            .When("it receives 2 points of 'damage'")
            .Then("it should have 1 'hp'")
            .Because("the same point can be modified multiple times by the same source if there are multiple modifiers");

        Given("it has 5 'hp'")
            .And("it receives -2 'hp' from 'damage'")
            .When("it receives 2 points of 'damage'")
            .Then("it should have 1 'hp'")
            .Because("it should lose double points when a double modifier is applied");

        Given("it has 5 'hp'")
            .And("it receives -1 'hp' from 'damage'")
            .When("it receives 8 points of 'damage'")
            .Then("it should have 0 'hp'")
            .Because("it should not lose more points than it has");

        Given("it has 5 'hp'")
            .And("it has a max 'hp' of 9")
            .And("it receives 1 'hp' from 'healing'")
            .When("it receives 5 points of 'healing'")
            .Then("it should have 9 'hp'")
            .Because("it should not go over its max points");

        Given("it has 5 'hp'")
            .And("it has 2 'shield'")
            .And("it receives -1 'hp' from 'damage' unless it has 'shield'")
            .When("it receives 5 points of 'damage'")
            .Then("it should have 5 'hp'")
            .Because("it should not lose points when another point is blocking them");

        Given("it has 5 'hp'")
            .And("it has 0 'shield'")
            .And("it receives -1 'hp' from 'damage' unless it has 'shield'")
            .When("it receives 5 points of 'damage'")
            .Then("it should have 0 'hp'")
            .Because("it should lose points if the shield is depleted");
    }

    public void ItHas____(float amount, string type)
    {
        it = transform.Require<HasPoints>();
        it.Set(type, amount);
    }

    public void ItReceives____From__(float modifier, string type, string source)
    {
        ReceivesPointsFromSource mod = it.gameObject.AddComponent<ReceivesPointsFromSource>();
        mod.type = type;
        mod.source = source;
        mod.modifier = modifier;
    }

    public void ItReceives____From__UnlessItHas__(float modifier, string type, string source, string blockedType)
    {
        ReceivesPointsUnlessItHas mod = it.gameObject.AddComponent<ReceivesPointsUnlessItHas>();
        mod.type = type;
        mod.source = source;
        mod.modifier = modifier;
        mod.unlessItHasPointsIn = blockedType;
    }

    public void ItReceives__PointsOf__(float amount, string type)
    {
        it.Deal(type, amount);
    }

    public void ItShouldHave____(float amount, string type)
    {
        it.Get(type).ShouldBe(amount);
    }

    public void ItHasAMax__Of__(string type, float amount)
    {
        it.SetMax(type, amount);
    }
}
