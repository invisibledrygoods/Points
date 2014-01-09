Points
======

Mostly in-house data helper for Video Geimus

Points
------

    HasPoints points = transform.Require<HasPoints>();

Get some hitpoints

    points.Set("hp", 100);

Set a max

    points.SetMax("hp", 150);

Take some damage

    var mod = AddComponent<ReceivesPointsFromSource>();
    mod.type = "hp";
    mod.source = "damage";
    mod.modifier = -1.0f;

    points.Deal("damage", 10);
    
Check your health

    points.Get("hp");

Heal yourself

    var mod = AddComponent<ReceivesPointsFromSource>();
    mod.type = "hp";
    mod.source = "healing";
    mod.modifier = 1.0f;
    
    points.Deal("healing", 10);
    
Buy a shield

    var mod = AddComponent<ReceivesPointsUnlessItHas>();
    mod.type = "hp";
    mod.source = "damage";
    mod.unlessItHasPointsIn = "shield";
    mod.modifier = -1.0f;

    points.Set("shield", 10);
    
Flags
-----

    HasFlags flags = transform.Require<HasFlags>();
   
Go down koujaku's route a bit

    flags.Raise("Hesitate");
    flags.Lower("WhyTheUmbrella");
    flags.Raise("FightBack");
    
Check if you're on the right track

    if (flags.Get("Hesitate") && !flags.Get("WhyTheUmbrella") && flags.Get("FightBack")) {
      Debug.Log("more like kouJERKu amirite?");
    }
    
Strings
-------

    HasStrings strings = transform.Require<HasStrings>();
    
Name your character

    strings.Set("name", "Aoba");
    strings.Set("suffix", "tan");
    
Show some dialog

    strings.Interpolate("Hi, I'm #{name}-#{suffix} and I have #{.hp} hitpoints.");
    
Saving and loading
------------------

I'm too tired to document this right now. Points, flags and strings all have Load and Save methods that write to pseudo-files in PlayerPrefs and behave as you would expect them.
