using UnityEngine;
using System.Collections;
using Shouldly;
using Require;
using System;

public class HasFlagsTest : TestBehaviour
{
    HasFlags it;

    public override void Spec()
    {
        Given("'RedTunic' is lowered")
            .When("it raises 'RedTunic'")
            .Then("'RedTunic' should be raised")
            .Because("raising a flag raises it");

        Given("'RedTunic' is raised")
            .When("it lowers 'RedTunic'")
            .Then("'RedTunic' should be lowered")
            .Because("lowering a flag lowers it");

        Given("'RedTunic' is raised")
            .When("'RedTunic' is raised")
            .Then("it should only have one flag")
            .Because("raising the same flag twice should only create one flag");
    }

    public void __IsLowered(string flag)
    {
        it = transform.Require<HasFlags>();
        it.Lower(flag);
    }

    public void __IsRaised(string flag)
    {
        it = transform.Require<HasFlags>();
        it.Raise(flag);
    }

    public void ItLowers__(string flag)
    {
        it.Lower(flag);
    }

    public void ItRaises__(string flag)
    {
        it.Raise(flag);
    }

    public void __ShouldBeLowered(string flag)
    {
        it.Get(flag).ShouldBe(false);
    }

    public void __ShouldBeRaised(string flag)
    {
        it.Get(flag).ShouldBe(true);
    }

    public void ItShouldOnlyHaveOneFlag()
    {
        it.flags.Count.ShouldBe(1);
    }
}
