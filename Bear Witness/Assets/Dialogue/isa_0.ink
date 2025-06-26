#isa_0

VAR loops = 0
VAR friend = 0
VAR plot_progress = 0
VAR wait_for_loop = false

EXTERNAL save(end)
EXTERNAL changeFriend(target, int)
EXTERNAL changeMoney(int)
EXTERNAL changeName(target, name)
EXTERNAL giveItem(name)

(THIS LINE WILL BE SKIPPED)

-> isa_greet

=== start ===
(SKIP)
-> isa_return

=== isa_greet ===
Oh! Hi, Arcas! #isa_happy
What brings you to the library? I thought you were busy getting ready for your expedition. #isa_
-> isa_choice

=== isa_end ===
^S
-> END

=== isa_return ===
{friend > 1: Can I do anything else for you? | Need anything?}
-> isa_choice

=== isa_choice ===
+ Just passing through. #arc_
    Well, it's nice to see you. Good luck today! #isa_happy
    -> isa_end
* {loops >= 1 and plot_progress < 1} I think I've been here before. #arc_worry
    What do you mean? You live here. #isa_worry
    No, like - I've gone back in time. Everything keeps repeating. #arc_worry
    Oh... I see. How long has this been happening? #isa_worry
    {
    - loops == 1: This is the first time I've been sent back. #arc_guilt
        Hmm. Well, we had better make sure this is a real loop. Let me know next time you come back. #isa_
        Right. #arc_
        -> isa_end
    - loops == 2: Twice now. Each time seems to be around ten minutes. #arc_conce
        Interesting. What happens at the end of the loop? #isa_
        -> isa_loop_end
    - loops > 2 and loops < 10: {loops} times so far. Ten minutes each. #arc_conce
        I see. What happens at the end of the loop? #isa_
        -> isa_loop_end
    - loops > 9: I've... kind of lost count. It's been a lot. #arc_worry
        Oh no. Have you talked to anyone about this yet? #isa_worry
        Well, yeah... but nobody really believes me. #arc_sad
        Oh no! Why didn't you come to me sooner? You can always count on me, you know. #isa_sad
        Ah, no matter. I'm here for you now. #isa_happy
        If you don't mind me asking, what happens at the end of the loop? #isa_
        -> isa_loop_end
    }
* {not wait_for_loop and plot_progress == 1} Pigeon. #arc_
    Time loop? #isa_
    Yep. We talked about how it ends. It stayed the same. #arc_
    Okay. Has anything changed between loops? #isa_
    For the most part, no. But I found an ice pick while I was on the mainland, and I was able to bring it back with me. #arc_
    Hmm. Is it still on the mainland? #isa_
    I'm not sure. #arc_conce
    Next time you get a chance, look for it and let me know. #isa_
    Understood. #arc_happy
    By the way, Isa... thanks for believing me. It... helps. #arc_guilt
    Oh! um... of course! #isa_embar
    -> isa_end
    
    === isa_loop_end ===
    I start... burning up. It feels like I'm dying. #arc_conce
    And then... I start to fall apart. #arc_worry
    I pass through this strange, dark space, and then I end up right back home. #arc_conce
    Hmm. Are you always in Arktis when it happens, or have you made it outside? #isa_
    Actually, it's only ever happened once I've left for the mainland. #arc_skept
    Interesting. You mentioned a burning sensation... perhaps the cold here keeps it in check? #isa_
    Well, keep working through the loop. Talk to me if anything strange happens. #isa_
    Oh! and - so you don't have to explain all this again, next time, say "pigeon". #isa_happy
    I'll know what you mean. #isa_happy
    ...Okay? #arc_skept
    ~plot_progress = 1
    ~wait_for_loop = true
    -> isa_end
    