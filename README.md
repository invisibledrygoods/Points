Points
======

Mostly in-house points helper for Video Geimus

Examples
--------

    HasPoints points = transform.Require<HasPoints>();

Get some hitpoints

    points.Set("hp", 100);

Set a max

    points.SetMax("hp", 150);

Take some damage

    points.SetModifier("hp", "damage", -1);
    points.Deal("damage", 10);
    
Check your health

    points.Get("hp");
    
Heal yourself

    points.SetModifier("hp", "healing", 1);
    points.Deal("healing", 10);
    
Buy a shield

    points.Set("shield", 10);
    points.SetBlock("shield", "damage", "hp");

Future Refactoring
==================

Currently this is one monolithic class with a complicated properties panel.
It should be broken up into multiple single purpose classes. i.e. ModifiesPoints,
BlocksPoints, etc.
