#lantern
EXTERNAL triggerFlag(name)
EXTERNAL giveItem(name)

VAR loops = 0

-> start

=== start ===
There's something inside... #arc_skept
Well, the axioms say I'm not supposed to damage any buildings. #arc_
{
- loops == 0: So I had better leave this be. #arc_
    ^S ->END
- But... will anyone know? It certainly would be useful... #arc_guilt
    + [Take it] Here goes... #arc_worry
        ~ triggerFlag("grab_lantern")
        ~ giveItem("Lantern")
        ^S -> END
    + [Do not] ...it's probably important. I'll leave it be. #arc_conce
        ^S -> END
}