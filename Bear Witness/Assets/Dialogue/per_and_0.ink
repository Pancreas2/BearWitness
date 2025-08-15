#per_and_0

EXTERNAL changeFriend(target, int)
EXTERNAL changeMoney(int)
EXTERNAL changeName(target, name)
EXTERNAL giveItem(name)

-> per_and_greet

=== per_and_greet ===
Hi Arcas! #and_happy
Arcas! How's it going? #per_happy
* [I'm doing great!] I've got this in the bag. #arc_happy
    That's my man! You're gonna crush this! #per_happy
    We're all rooting for you, Arcas! #and_happy
* [I'm a bit nervous.] To be perfectly honest, I'm a bit nervous. #arc_sad
    Don't worry, I'm sure you'll be fine. #and_happy
    Yeah, you'll get the hang of it. Think of it like the first time Orion took us seal hunting! #per_skept
    Yeah! You were so worried about falling into the hole. #and_happy
    But now look at you! You're one of the best hunters in the village! #per_happy
* [I have no idea what I'm doing please help] I have no idea what I'm doing. This is going to go horribly. #arc_worry
    Hey, nobody's first expedition is easy. #and_sad
    You remember Percy's? He had barely left the shore when-- #and_happy
    SHH! Don't listen to her, Arcas. #per_guilt
    Just take a deep breath and remember your mission. #per_
    ...and make sure to hold on to your iceberg. #per_guilt
- -> per_and_end

=== per_and_end ===
^S
-> END

=== per_and_return ===
You've got this, Arcas! #and_happy
Good luck! #per_happy
-> per_and_end

=== start ===
{-> per_and_greet |-> per_and_return }