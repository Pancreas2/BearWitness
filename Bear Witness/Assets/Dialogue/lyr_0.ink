#lyr_0

VAR loops = 0
VAR friend = 0
VAR plot_progress = 0
VAR wait_for_loop = false

EXTERNAL save(end)
EXTERNAL changeFriend(target, int)
EXTERNAL changeMoney(int)
EXTERNAL changeName(target, name)
EXTERNAL giveItem(name)
EXTERNAL progressPlot(int)
EXTERNAL seenLine(line)
EXTERNAL rememberLine(line)

-> lyr_greet

=== start ===
{-> lyr_greet |-> lyr_return}

=== lyr_greet ===
{
- plot_progress == 3: \( I don't... really want to talk to Lyra right now. Maybe someone else knows about rogue Witnesses? ) #arc_guilt
    \( I guess there's always the ^CYArkives^CX up past town... but Ophiuchus will {not seenLine("ophiuchus_talk"): probably} be there. Is it worth it? ) #arc_conce
    -> lyr_end
- Oh! Hi, Arcas! #lyr_happy
    What brings you to the library? I thought you were busy getting ready for your expedition. #lyr_
    -> lyr_choice
}

=== lyr_end ===
^S
-> END

=== lyr_return ===
{
- plot_progress == 3 and wait_for_loop: ... #lyr_sad
    -> lyr_end
- plot_progress == 3: \( I don't... really want to talk to Lyra right now. Maybe someone else knows about rogue Witnesses? ) #arc_guilt
    \( I guess there's always the ^CYArkives^CX up past town... but Ophiuchus will {not seenLine("ophiuchus_talk"): probably} be there. Is it worth it? ) #arc_conce
    -> lyr_end
- friend > 1: Can I do anything else for you? #lyr_
- Need anything? #lyr_
}
-> lyr_choice

=== lyr_choice ===
+ Just passing through. #arc_
    Well, it's nice to see you. Good luck today! #lyr_happy
    -> lyr_end
* {loops >= 1 and plot_progress < 1} I think I've been here before. #arc_worry
    What do you mean? You live here. #lyr_worry
    No, like - I've gone back in time. {loops < 3: I'm repeating the day. |Everything keeps repeating.} #arc_worry
    Oh... I see. How long has this been happening? #lyr_worry
    ~rememberLine("lyra_loop")
    {
    - loops == 1: This is the first time I've been sent back. #arc_guilt
        Hmm. Well, we had better make sure this is a real loop. Let me know next time you come back. #lyr_
        Right. I guess it could have been a weird dream. #arc_
        -> lyr_end
    - loops == 2: Twice now. Each time seems to be around ten minutes. #arc_conce
        Interesting. What happens at the end of the loop? #lyr_
        -> lyr_loop_end
    - loops > 2 and loops < 10: {loops} times so far. Ten minutes each. #arc_conce
        I see. What happens at the end of the loop? #lyr_
        -> lyr_loop_end
    - loops > 9: I've... kind of lost count. It's been a lot. #arc_worry
        Oh no. Have you talked to anyone about this yet? #lyr_worry
        Well, yeah... but nobody really believes me. #arc_sad
        Oh no! Why didn't you come to me sooner? You can always count on me, you know. #lyr_sad
        Ah, it's okay. I'm here for you now. #lyr_happy
        If you don't mind me asking, what happens at the end of the loop? #lyr_
        -> lyr_loop_end
    }
* {not wait_for_loop and plot_progress == 1} Pigeon? #arc_skept
    Time loop? #lyr_
    Yep. We talked about how it ends. It stayed the same. #arc_
    Okay. Has anything changed between loops? #lyr_
    For the most part, no. But I found an ice pick while I was on the mainland, and I was able to bring it back with me. #arc_
    Hmm. Is there a version of it still on the mainland? #lyr_
    I'm not sure. #arc_conce
    Next time you get a chance, look for it and let me know. #lyr_
    Understood. #arc_happy
    By the way, Isa... thanks for believing me. It... helps. #arc_guilt
    Oh! um... of course! #lyr_embar
    ~progressPlot(2)
    ~plot_progress = 2
    ~wait_for_loop = true
    -> lyr_end
* {not wait_for_loop and plot_progress == 2} Pigeon. #arc_
    Time loop? #lyr_
    Yep. We talked about how it ends, and what changes between loops. #arc_
    Okay. And you don't know why it's happening? #lyr_
    No, not.. not really. #arc_conce
    I remember a strange... dream, almost. From before the first loop. #arc_
    I was watching someone - a Witness, I think, but they... #arc_conce
    ... They destroyed an artifact. #arc_worry
    What? But that would go against... #lyr_worry
    Pretty much all of our axioms, yeah. So unless one of us has gone rogue, it must have just been a weird dream. #arc_conce
    ... yeah ... #lyr_worry
    What, ^ihas^I someone gone rogue? #arc_joke
    ... well, I didn't really want to get into it, but... #lyr_guilt
    ^CYCallisto^CX went missing on a trip to Corendia, right? #lyr_sad
    You don't mean... #arc_conce
    I'm sorry! This is why I didn't want to talk about it. #lyr_sad
    It's just, I just thought, I wanted to help... #lyr_sad
    No. This is just a stupid dream. ^CYMy mother^CX has nothing to do with this. #arc_angry
    And... you don't get to just... accuse her of something like that, when she isn't even... #arc_cry
    Arcas, I'm sorry, I'm sorry! I... we all miss her, you know? #lyr_sad
    ... #arc_cry
    ~progressPlot(3)
    ~plot_progress = 3
    ~wait_for_loop = true
    -> lyr_end
    
    === lyr_loop_end ===
    I start... burning up. It feels like I'm dying. #arc_conce
    And then... I start to fall apart. #arc_worry
    I pass through this strange, dark space, and then I end up right back home. #arc_conce
    Hmm. Are you always in Arktis when it happens, or have you made it outside? #lyr_
    Actually, it's only ever happened once I've left for the mainland. #arc_skept
    Interesting. You mentioned a burning sensation... perhaps the cold here keeps it in check? #lyr_
    Well, keep working through the loop. Talk to me if anything strange happens. #lyr_
    Oh! and - so you don't have to explain all this again, next time, say "pigeon". #lyr_happy
    I'll know what you mean. #lyr_happy
    ...Okay? #arc_skept
    ~plot_progress = 1
    ~progressPlot(1)
    ~wait_for_loop = true
    -> lyr_end
    