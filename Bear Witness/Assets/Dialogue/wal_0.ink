#wal_0


EXTERNAL rememberLine(line)
EXTERNAL seenLine(line)

-> start

=== start ===
VAR loopAdmission = false
~ loopAdmission = seenLine("master_looping_0")
Good morning, Arcas. #wal_
{Is there something on your mind?| Is there anything else you wish to ask of me?} #wal_
+ [No] No, sir. Ready to get started! #arc_happy
    Very well. #wal_
* [Why must I leave no trace?] I know we've gone over this before, but I wanted to clarify - why is it so important that we leave no trace? #arc_nervo
    Hm. It comes down to the very nature of observation, and the fundamental impossibility of our task as chroniclers. #wal_
    To observe something is to change it. For instance, we can never know what happens when no one is watching. #wal_
    And yet, our directive demands that we observe the world as it is, so we must strive to attain this ideal. #wal_sad
    Our existence must remain a secret, lest the world learn it is being watched, and change accordingly. #wal_sad
    I see. So the important part is that we don't change the course of history? #arc_
    In essence, yes. #wal_
    ~ rememberLine("no_trace_0")
    {not mission: Well, I'm pleased to offer you assistance before your expedition.} #wal_
    -> mission
* {not loopAdmission} [I'm looping through time.] I - #arc_nervo
    \( No, I can't tell Master about this. It's been... helpful, really. And I doubt he'd let me leave if he knew. ) #arc_nervo
    {not seenLine("lyra_loop")} \( But nobody else would believe me... except maybe ^CBLyra^CX? She's probably holed up in the library as usual. ) #arc_skept
    ... I'm just excited to get started, sir! #arc_grin
    ~ rememberLine("master_looping_0")
    Very well. #wal_
- -> mission

=== mission ===
{ stopping:
    - I've decided you are to go to Corendia. Legends tell of a mysterious ^CY power source^CX hidden deep in an ^CYancient ruin.^CX #wal_
        You are to secure this power source and bring it home. #wal_
        Yes, sir! I'll get straight to it. #arc_happy
    - Now, I do not mean to rush you, young one, but your expedition awaits. #wal_
        Right. #arc_
}
^S
-> END
