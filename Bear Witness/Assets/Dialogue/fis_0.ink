#fis_0

(SKIPPED TEXT)

VAR money = 0
VAR friend = 0
VAR time = 0

EXTERNAL changeFriend(target, int)
EXTERNAL changeMoney(int)
EXTERNAL changeName(target, name)
EXTERNAL giveItem(name)

{fis_end: -> fis_further_greet |-> fis_greet}


=== fis_greet ===
Ay, laddie. What brings ye here? #fis_happy
Good morning! #arc_happy
Yeh're a Witness, aren't ye? #fis_skept
Uhhh... #arc_nervo
* [I am.] I'm not really supposed to tell you... but yes. #arc_sad
    But you'd already figured it out, hadn't you? #arc_skept
    Of course, laddie. Me old pa told us wee cubs legends about ye. #fis_happy
    'Ts an honor, really, meeting ye. #fis_happy
    ~ friend++
    ~ changeFriend("fis", 1)
    -> fis_main
* Why do you ask? #arc_skept
    Look, laddie. An iceberg wit' a pick in't washes up this morn', some bear I ain't never seen before comes by, and 'is fur's a white the likes none'f'us've seen in years. #fis_skept
    Yeh're a Witness. #fis_
    ** I can't really hide it, can I? #arc_conce
        Ach, 't's okay, laddie. Piece of advice, though: cover up that fur of yers. t's a dead giveaway. #fis_
        Some of us out 'ere don't take so kindly to yer like. Don't like their secrets being pilfered. #fis_
        Take care, laddie. #fis_
        ~ friend++
        ~ changeFriend("fis", 1)
        -> fis_end
    ** [Please don't tell anyone.] Yeah, you got me. I'm not really supposed to tell anyone. Can you keep it a secret? #arc_embar
        No worries, laddie. Won't tell a soul. #fis_happy
        Jus' 'appy an ol' timer like me won't 'ave the wool pulled over 'is eyes so eas'ly. #fis_happy
        ~ friend++
        ~ changeFriend("fis", 1)
        -> fis_end
    ** [... No I'm not!] You must have made a mistake. I'm not a Witness. #arc_disda
        If ye say so, laddie. But if ye happen to see the Witness, which I'm taken to understand yeh're not, tell 'em 'bout the ice pick, nay? #fis_skept
        I'm taken to believe it's a poor Witness indeed, leaving evidence of 'eir arrival like such. #fis_
        ... (hey, that's not even my fault!) #arc_hurt
        ~ friend--
        ~ changeFriend("fis", -1)
        -> fis_end
* [No, I'm not.] Uh.. no. #arc_guilt
    Ach, I see. My mistake. #fis_skept
    Well, if ye see a Witness then, tell 'em to look westwards for their ice pick. #fis_
    I'm taken to believe it's a poor Witness indeed, leaving evidence of 'eir arrival like such. #fis_
    Uh.. yeah, I'll do that. #arc_hurt
    ~ friend--
    ~ changeFriend("fis", -1)
    -> fis_end

=== fis_end ===
^S 
-> END

=== fis_main ===
{240 < time < 390: Ah, and laddie - take cover soon, nay? I wager there's a storm a-comin'.} #fis_
-> fis_end

=== fis_further_greet ===
{
- friend > 0: 'Lo there, laddie! #fis_happy
    -> fis_main
- friend < 0: Good day. #fis_
    -> fis_end
- 'Lo there. #fis_
    -> fis_main
}

=== start ===
{fis_end: -> fis_further_greet |-> fis_greet}
