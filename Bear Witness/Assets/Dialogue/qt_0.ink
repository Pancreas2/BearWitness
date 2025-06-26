#qt_0

EXTERNAL changeFriend(target, int)
EXTERNAL changeMoney(int)
EXTERNAL changeName(target, name)
EXTERNAL giveItem(name)

(THIS LINE WILL BE SKIPPED)

-> qt_greet

=== qt_greet ===
Hi Arcas! #tal_happy
Arcas! How's it going? #que_happy
* [I'm doing great!] I've got this in the bag. #arc_happy
    That's my man! You're gonna crush this! #que_happy
    We're all rooting for you, Arcas! #tal_happy
* [I'm a bit nervous.] To be perfectly honest, I'm a bit nervous. #arc_sad
    Don't worry, I'm sure you'll be fine. #tal_happy
    Yeah, you'll get the hang of it. Think of it like the first time Pallas took us seal hunting! #que_skept
    Yeah! You were so worried about falling into the hole. #tal_happy
    But now look at you! You're one of the best hunters in the village! #que_happy
* [I have no idea what I'm doing please help] I have no idea what I'm doing. This is going to go horribly. #arc_worry
    Hey, nobody's first expedition is easy. #tal_sad
    You remember Quentin's? He had barely left the shore when-^B #tal_happy
    SHH! Don't listen to her, Arcas. #que_guilt
    Just take a deep breath and remember your mission. #que_
    ...and make sure to hold on to your iceberg. #que_guilt
- -> qt_end

=== qt_end ===
^S
-> END

=== start ===
(THIS LINE WILL BE SKIPPED)
You've got this, Arcas! #tal_happy
Good luck! #que_happy
-> qt_end