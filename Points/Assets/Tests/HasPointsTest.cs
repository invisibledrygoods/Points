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
            .Then("it should have 2 'hp'");

        Given("it has 5 'hp'")
            .And("it receives -2 'hp' from 'damage'")
            .When("it receives 2 points of 'damage'")
            .Then("it should have 1 'hp'");

        Given("it has 5 'hp'")
            .And("it receives -1 'hp' from 'damage'")
            .When("it receives 8 points of 'damage'")
            .Then("it should have 0 'hp'");

        Given("it has 5 'hp'")
            .And("it has a max 'hp' of 9")
            .And("it receives 1 'hp' from 'healing'")
            .When("it receives 5 points of 'healing'")
            .Then("it should have 9 'hp'");

        Given("it has 5 'hp'")
            .And("it has a 2 'shield'")
            .And("the 'shield' blocks 'damage' to 'hp'")
            .And("it receives -1 'hp' from 'damage'")
            .When("it receives 5 points of 'damage'")
            .Then("it should have 5 'hp'");

        Given("it has 5 'hp'")
            .And("it has a 0 'shield'")
            .And("the 'shield' blocks 'damage' to 'hp'")
            .And("it receives -1 'hp' from 'damage'")
            .When("it receives 5 points of 'damage'")
            .Then("it should have 5 'hp'");
    }
}
