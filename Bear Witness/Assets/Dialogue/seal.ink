#seal
(Skipped)

VAR friend = 0

EXTERNAL changeFriend(id, mod)

I'm hiding from the bears! #sea_happy
Want to hide with me? #sea_happy
* I'm a bear. #arc_disda
    Oh no! #sea_sad
    I hope you don't find me! #sea_sad
    ~ changeFriend("sea", -1)
    ~ friend++
* Sure! #arc_happy
    Yay! #sea_happy
    I hope they don't find us! #sea_happy
    ~ changeFriend("sea", 1)
    ~ friend++
- ^S -> END

=== start ===
(SKIP)
{
- friend > 0: Be careful out there, a bear might find you! #sea_happy
    ^S -> END
- friend < 0: I sure am lucky I haven't been found yet! #sea_happy
    ^S -> END
- How did you get here? #sea_sad
    ^S -> END
}