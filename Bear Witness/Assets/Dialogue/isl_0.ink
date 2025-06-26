#isl_0

(Skip this line)

EXTERNAL hasItem(name)

Hey, Arcas! Ready for the big day? #isl_happy
I've been working on a special gift for you... #isl_happy
...shoot. I must have misplaced it. #isl_conce
* [Don't worry about it.] That's okay, dad. I'm just excited to get started! #arc_happy
    That's my boy! Knock 'em dead. #isl_happy
    You know, figuratively. #isl_conce
    You got it! #arc_happy
    -> isl_end
* {hasItem("Ice Pick")} [(Show Ice Pick)] Oh! You found it! #isl_happy
    -> isl_found_pick
* {not hasItem("Ice Pick")} [Yeah, right.] Come on, dad, don't tease me like that. #arc_joke
    No, I'm serious, it must be here somewhere... #isl_conce
    -> isl_ice_pick
* [What was it?] What were you making? #arc_skept
    -> isl_ice_pick



=== isl_ice_pick ===
I wanted to make you an Ice Pick. It was your mother's signature tool, you know... #isl_
...oh. Thanks, dad. That would've been nice. #arc_worry
...well, I'm sure it'll turn up sooner or later. Good luck on your expedition! #isl_happy
-> isl_end

=== isl_found_pick ===
I'm sorry, it was meant to be a surprise. #isl_embar
Where was it, then? #isl_
I found it by the shore in mainland Corendia. #arc_guilt
Huh? But you haven't been yet... How long was I sleeping? #isl_conce
I didn't hibernate through your big day again, did I? #isl_shock
No... no, dad, you're fine. #arc_sad
-> isl_end

=== isl_end ===
^S
-> END

=== start ===
(SKIPPED)
{
    - isl_found_pick: ... #isl_sad
    - Take care! #isl_
}
-> isl_end