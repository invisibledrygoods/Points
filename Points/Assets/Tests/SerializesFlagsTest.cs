using UnityEngine;
using System.Collections;
using Require;
using Shouldly;

public class SerializesFlagsTest : TestBehaviour
{
    HasFlags it;
    string serialized;

    public override void Spec()
    {
        Given("'RedTunic' is raised and savable")
            .And("'MasterSword' is lowered and savable")
            .When("it is serialized")
            .Then(@"it should be serialized as 'RedTunic:True,MasterSword:False'")
            .Because("serialization should produce a hash");

        Given("'RedTunic' is lowered and savable")
            .And("'MasterSword' is lowered and savable")
            .When("it deserializes 'RedTunic:True,MasterSword:True'")
            .Then("'RedTunic' should be raised")
            .And("'MasterSword' should be raised")
            .Because("it should load hashes correctly");

        Given("'RedTunic' is lowered and unsavable")
            .And("'MasterSword' is lowered and savable")
            .When("it is serialized")
            .Then("it should be serialized as 'MasterSword:False'")
            .Because("unsavable things shouldn't be saved");

        Given("'RedTunic' is lowered and unsavable")
            .And("'MasterSword' is lowered and savable")
            .When("it deserializes 'RedTunic:True,MasterSword:True'")
            .Then("'RedTunic' should be lowered")
            .Because("it shouldn't load unsavable data");
    }

    public void __IsRaisedAndSavable(string name)
    {
        it = it ?? transform.Require<HasFlags>();
        it.Raise(name);
        it.SetSavable(name, true);
    }

    public void __IsLoweredAndSavable(string name)
    {
        it = it ?? transform.Require<HasFlags>();
        it.Lower(name);
        it.SetSavable(name, true);
    }

    public void __IsRaisedAndUnsavable(string name)
    {
        it = it ?? transform.Require<HasFlags>();
        it.Raise(name);
        it.SetSavable(name, false);
    }

    public void __IsLoweredAndUnsavable(string name)
    {
        it = it ?? transform.Require<HasFlags>();
        it.Lower(name);
        it.SetSavable(name, false);
    }

    public void ItIsSerialized()
    {
        serialized = it.Serialize();
    }

    public void ItDeserializes__(string serialized)
    {
        it.Deserialize(serialized);
    }

    public void ItShouldBeSerializedAs__(string expected)
    {
        serialized.ShouldBe(expected);
    }

    public void __ShouldBeRaised(string name)
    {
        it.Get(name).ShouldBe(true);
    }

    public void __ShouldBeLowered(string name)
    {
        it.Get(name).ShouldBe(false);
    }
}
