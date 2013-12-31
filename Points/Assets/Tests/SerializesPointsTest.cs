using UnityEngine;
using System.Collections;
using Require;
using Shouldly;

public class SerializesPointsTest : TestBehaviour
{
    HasPoints it;
    string serialized;

    public override void Spec()
    {
        Given("it has 5 savable 'hp' points")
            .And("it has 3 savable 'mp' points")
            .When("it is serialized")
            .Then("it should be serialized as 'hp:5,mp:3'")
            .Because("serialization should produce a hash");

        Given("it has 0 savable 'hp' points")
            .And("it has 0 savable 'mp' points")
            .When("it deserializes 'hp:5,mp:3'")
            .Then("it should have 5 'hp' points")
            .And("it should have 3 'mp' points")
            .Because("it should load hashes correctly");

        Given("it has 0 unsavable 'hp' points")
            .And("it has 0 savable 'mp' points")
            .When("it is serialized")
            .Then("it should be serialized as 'mp:0'")
            .Because("unsavable things shouldn't be saved");

        Given("it has 0 unsavable 'hp' points")
            .And("it has 0 savable 'mp' points")
            .When("it deserializes 'hp:5,mp:3'")
            .Then("it should have 0 'hp' points")
            .Because("it shouldn't load unsavable data");

        Given("it has 10 savable 'hp' points")
            .And("its max 'hp' is 20")
            .When("it is serialized")
            .Then("it should be serialized as 'hp:10/20'")
            .Because("it should save point maximums");

        Given("it has 0 savable 'hp' points")
            .When("it deserializes 'hp:20/30'")
            .Then("it should have 30 max 'hp'")
            .Because("it should load point maximums");
    }

    public void ItHas__Savable__Points(float amount, string type)
    {
        it = it ?? transform.Require<HasPoints>();
        it.Set(type, amount);
        it.SetSavable(type, true);
    }

    public void ItHas__Unsavable__Points(float amount, string type)
    {
        it = it ?? transform.Require<HasPoints>();
        it.Set(type, amount);
        it.SetSavable(type, false);
    }

    public void ItsMax__Is__(string type, float max)
    {
        it.SetMax(type, max);
    }

    public void ItDeserializes__(string serialized)
    {
        it.Deserialize(serialized);
    }

    public void ItIsSerialized()
    {
        serialized = it.Serialize();
    }

    public void ItShouldBeSerializedAs__(string expected)
    {
        serialized.ShouldBe(expected);
    }

    public void ItShouldHave____Points(float amount, string type)
    {
        it.Get(type).ShouldBe(amount);
    }

    public void ItShouldHave__Max__(float amount, string type)
    {
        it.GetMax(type).ShouldBe(amount);
    }
}
