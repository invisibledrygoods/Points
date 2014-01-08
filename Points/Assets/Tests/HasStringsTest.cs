using UnityEngine;
using System.Collections;
using Shouldly;
using Require;
using System;

public class HasStringsTest : TestBehaviour
{
    HasStrings it;
    HasPoints points;
    string interpolated;

    public override void Spec()
    {
        Given("'playerOne' is 'throthgar'")
            .When("'playerOne' is changed to 'bothfar'")
            .Then("'playerOne' should be 'bothfar'")
            .Because("changing a string changes it");

        Given("'playerOne' is 'throthgar'")
            .When("'playerOne' is changed to 'bothfar'")
            .Then("there should be only one string")
            .Because("changing a string should not add it twice");

        Given("'playerOne' is 'throthgar'")
            .When("i say 'hi #{playerOne}, have you seen any dragons around?'")
            .Then("it should be interpolated as 'hi throthgar, have you seen any dragons around?'")
            .Because("ruby style interpolation should work");

        Given("'playerOne' is 'throthgar'")
            .When("i say 'hi #{playerTwo}, have you seen #{playerOne} around?'")
            .Then("it should be interpolated as 'hi #{playerTwo}, have you seen throthgar around?'")
            .Because("unknown keys should not be interpolated");

        Given("'playerOne' is 'throthgar'")
            .And("he has 12 'gold' points")
            .When("i say '#{playerOne}! #{.gold} gold is not enough to buy a sword!'")
            .Then("it should be interpolated as 'throthgar! 12 gold is not enough to buy a sword!'")
            .Because("#{.} should interpolate from points instead of strings");

        Given("'playerOne' is 'throthgar'")
            .And("he has 12.5 'gold' points")
            .When("i say '#{playerOne}! #{.gold} gold is not enough to buy a sword!'")
            .Then("it should be interpolated as 'throthgar! 12 gold is not enough to buy a sword!'")
            .Because("#{.} should truncate floating points by default");

//        I don't like these syntaxes but might want the features

//        Given("'playerOne' is 'throthgar'")
//            .And("he has 12 'gold' points")
//            .When("i say '#{playerOne}! #{2.gold} gold is not enough to buy a sword!'")
//            .Then("it should be interpolated as 'throthgar! 12.00 gold is not enough to buy a sword!'")
//            .Because("#{x.} should summon x machine eidolons")

//        Given("'playerOne' is 'throthgar'")
//            .And("'lostSword' is raised")
//            .When("i say '#{lostSword?I see youve lost your sword:Nice sword}, #{playerOne}!'")
//            .Then("it should be interpolated as 'I see youve lost your sword, throthgar!")
//            .Because("#{?:} should use flags as a ternary operation");
    }

    public void __Is__(string name, string text)
    {
        it = transform.Require<HasStrings>();
        it.Set(name, text);
    }

    public void __IsChangedTo__(string name, string text)
    {
        it.Set(name, text);
    }

    public void ISay__(string interpolate)
    {
        interpolated = it.Interpolate(interpolate); 
    }

    public void HeHas____Points(float amount, string type)
    {
        points = transform.Require<HasPoints>();
        points.Set(type, amount);
    }

    public void __ShouldBe__(string name, string expected)
    {
        it.Get(name).ShouldBe(expected);
    }

    public void ThereShouldBeOnlyOneString()
    {
        it.strings.Count.ShouldBe(1);
    }

    public void ItShouldBeInterpolatedAs__(string expected)
    {
        interpolated.ShouldBe(expected);
    }
}
