#ori_0

EXTERNAL hasTool(name)
VAR hasPick = false
VAR loops = 0
~ hasPick = hasTool("Ice Pick")

=== ori_greet ===
Hey, Arcas! Ready for the big day? #ori_happy
I've been working on a special gift for you... #ori_happy
-> ori_choice

=== ori_choice ===
...shoot. I must have misplaced it. #ori_conce
* [Don't worry about it.] That's okay, dad. I'm just excited to get started! #arc_happy
    That's my boy! Knock 'em dead. #ori_happy
    You know, figuratively. #ori_conce
    You got it! #arc_happy
    -> ori_end
* {hasPick} [(Show Ice Pick)] Oh! You found it! #ori_happy
    -> ori_found_pick
* {not hasPick} [Yeah, right.] Come on, dad, don't tease me like that. #arc_joke
    No, I'm serious, it must be here somewhere... #ori_conce
    -> ori_ice_pick
* {loops > 0} I think I've been here before. #arc_worry
    -> ori_tell_loop
* [What was it?] What were you making? #arc_skept
    -> ori_ice_pick


=== ori_ice_pick ===
I wanted to make you an Ice Pick. It was your mother's signature tool, you know... #ori_
...oh. Thanks, dad. That would've been nice. #arc_worry
...well, I'm sure it'll turn up sooner or later. Good luck on your expedition! #ori_happy
-> ori_end

=== ori_found_pick ===
I'm sorry, it was meant to be a surprise. #ori_embar
Where was it, then? #ori_
I found it by the shore in mainland Corendia. #arc_guilt
Huh? But you haven't been yet... How long was I sleeping? #ori_conce
I didn't hibernate through your big day again, did I? #ori_shock
No... no, dad, you're fine. #arc_sad
-> ori_end

=== ori_end ===
^S
-> END

=== ori_tell_loop ===
Well, yes, of course you have? You were here helping me clean up yesterday. #ori_conce
No, I mean... I've been through this day before. #arc_worry
... #ori_conce
... Oh, I see. You were probably running everything through your head before the big day, huh? I get it. #ori_
No, it... #arc_worry
Don't worry, champ. It just means you're well-prepared. #ori_happy
( Well, I guess it does... just maybe not the way you're thinking. ) #arc_
... thanks, dad. #arc_guilt
-> ori_end

=== ori_return ===
{
    - ori_found_pick: ... #ori_sad
    - ori_tell_loop: I... wish I could help you more. #ori_guilt
        But I know you can handle this. #ori_
    - Take care! #ori_
}
-> ori_end

=== start ===
{ -> ori_greet |-> ori_return }

-> ori_end