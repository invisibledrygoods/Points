using UnityEngine;
using System.Collections;
using Require;
using Shouldly;

public class SerializesStringsTest : TestBehaviour
{
    HasStrings it;
    string serialized;

    public override void Spec()
    {
        Given("'playerOne' is 'throthgar the brave' and savable")
            .And("'playerTwo' is 'bothfar' and savable")
            .When("it is serialized")
            .Then("it should be serialized as 'playerOne:throthgar the brave,playerTwo:bothfar'")
            .Because("serialization should produce a hash");

        Given("'playerOne' is 'whatever' and savable")
            .And("'playerTwo' is 'whatever' and savable")
            .When("it deserializes 'playerOne:throthgar,playerTwo:bothfar'")
            .Then("'playerOne' should be 'throthgar'")
            .And("'playerTwo' should be 'bothfar'")
            .Because("it should load hashes correctly");

        Given("'playerOne' is 'throthgar' and unsavable")
            .And("'playerTwo' is 'bothfar' and savable")
            .When("it is serialized")
            .Then("it should be serialized as 'playerTwo:bothfar'")
            .Because("unsavable things shouldn't be saved");

        Given("'playerOne' is 'whatever' and unsavable")
            .And("'playerTwo' is 'whatever' and savable")
            .When("it deserializes 'playerOne:throthgar,playerTwo:bothfar'")
            .Then("'playerOne' should be 'whatever'")
            .Because("it shouldn't load unsavable data");

            Given("'playerTwo' is 'nobody' and savable")
            .And("'playerOne' is 'throthgar,playerTwo:bothfar' and savable")
            .And("it is serialized")
            .When("it is deserialized")
            .Then("'playerOne' should be 'throthgar,playerTwo:bothfar'")
            .And("'playerTwo' should be 'nobody'")
            .Because("it shouldn't fall for injection attacks");
    }

    public void __Is__AndSavable(string name, string text)
    {
        it = it ?? transform.Require<HasStrings>();
        it.Set(name, text);
        it.SetSavable(name, true);
    }

    public void __Is__AndUnsavable(string name, string text)
    {
        it = it ?? transform.Require<HasStrings>();
        it.Set(name, text);
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

    public void ItIsDeserialized()
    {
        it.Deserialize(serialized);
    }

    public void ItShouldBeSerializedAs__(string expected)
    {
        serialized.ShouldBe(expected);
    }

    public void __ShouldBe__(string name, string text)
    {
        it.Get(name).ShouldBe(text);
    }
}
