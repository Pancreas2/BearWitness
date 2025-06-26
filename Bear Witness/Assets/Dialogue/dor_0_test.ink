#dor_0

VAR money = 0
VAR friend = 0
VAR didsomething = false

EXTERNAL save(end)
EXTERNAL changeFriend(target, int)
EXTERNAL changeMoney(int)
EXTERNAL changeName(target, name)
EXTERNAL giveItem(name)
EXTERNAL openDoor(name)

(THIS LINE WILL BE SKIPPED)

-> dor_at_lighthouse

=== dor_at_lighthouse ===
= dor_greet
Hello and welcome to Topiary Airlines, the top-(i)-air-(y)-line in all of Corendia. #dor_bored
{friend >= 1 and not dor_canned: -> dor_canned|-> dor_main}
= dor_main
{What can I do for you today?|Anything else I can help you with?} #dor_bored
* What is Topiary Airlines? #arc_
    -> dor_airline
* {dor_airline and not dor_boredom} You seem bored. #arc_
    -> dor_boredom
* {dor_airline and dor_boredom and not dor_tumble} [Why is it called the Tumbleweed?] Why would you name an airship the 'Tumbleweed'? #arc_skept
    -> dor_tumble
+ {not dor_pay_ticket} [I'd like to buy a ticket.] One ticket, please. #arc_happy
    -> dor_buy_ticket
+ {dor_pay_ticket} [I'd like to buy a ticket.] I'd like another ticket, please. #arc_happy
    {not dor_pay_ticket_again: -> dor_buy_ticket_again| {dor_pay_ticket_again > 2: -> dor_buy_eternal_tickets| -> dor_buy_ticket_yet_again}}
+ {friend < 2 or didsomething} [Nothing, thanks.] {didsomething: That's all for now, thanks.|I'm good for now, thanks.} #arc_happy
    -> dor_bye
+ {friend >= 2 and not didsomething} [Nothing, thanks.] Just saying hi! #arc_happy
    -> dor_awkward_hi
    
= dor_bye
Topiary Airlines thanks you for your patronage. #dor_bored
-> dor_end

= dor_awkward_hi
Oh! Well... hi! #dor_embar
Hi! #arc_happy
-> dor_end

= dor_buy_ticket
~ didsomething = true
That'll be 200$. #dor_bored
{money < 200: -> dor_cant_afford}
+ [Buy] Here you are. #arc_happy
    -> dor_pay_ticket
+ [Do not] On second thought - I think I'll pass, thanks. #arc_
    No worries, I get it. #dor_bored
    -> dor_main
    
= dor_buy_ticket_again
~ didsomething = true
But you already have one! Your ticket's good for the whole day. #dor_worry
Are you travelling with someone? #dor_worry
No. #arc_happy
But... then... #dor_worry
+ I said I'd like another ticket, please. #arc_disda
    Um... okay. I guess I can..? #dor_worry
    -> dor_pay_ticket_again
+ [Nevermind.] Ah, I guess I don't really need another. #arc_
    ...okay. #dor_conce
    -> dor_main
    
= dor_buy_ticket_yet_again
~ didsomething = true
No! You have two tickets! #dor_worry
{
- money >= 200: Why not? I can afford it. Can you stop me? #arc_joke
    -> dor_pay_ticket_again
- I'm only joking, I can't afford another. #arc_joke
    -> dor_main
} 

= dor_buy_eternal_tickets
~ didsomething = true
{\(Not again...)|(Of course.)|(Should have guessed.)} #dor_bored
-> dor_pay_ticket_again

= dor_pay_ticket
~ changeMoney(-200)
Thanks! and here's your ticket. #dor_happy
~ giveItem("Airship_Ticket")
~ openDoor("Lighthouse_Airship")
Thank you! #arc_happy
-> dor_main

= dor_pay_ticket_again
{\(I can't believe I'm doing this.)|No...|(What are these tickets even for?!)|...|Sure. Why not.} #dor_bored
~ changeMoney(-200)
~ giveItem("Airship_Ticket")
Thank you! #arc_happy
-> dor_main

= dor_cant_afford
I don't think I have enough. #arc_conce
Yeah, been there. #dor_bored
Trust me, you're not missing out. It takes a bit longer to walk to the city, but not much. #dor_
-> dor_main

= dor_airline
~ didsomething = true
We're the leading air travel company on Corendia Island. Our ship, the Tumbleweed, tours the island twice a day. #dor_bored
* You seem bored. #arc_
    -> dor_boredom
* [Why is it called the Tumbleweed?] Why would you name an airship the 'Tumbleweed'? #arc_skept
    -> dor_tumble
+ Thanks, that clears it up. #arc_
    -> dor_main
    
= dor_boredom
~ didsomething = true
Oh! Um... no.. I... well... don't tell my boss. #dor_worry
* [Of course not.] I don't even know who your boss is. But I won't tell anyone, just to be safe. #arc_grin
    Ha! Yeah, I guess I shouldn't be so worried. #dor_happy
    I guess I'm bored because nothing ever happens here, you know? #dor_
    It's just the same knobby customers every day, and I can't help but feel like I'm wasting my life. #dor_worry
    But... thanks for asking. It breaks up the monotony, at least. #dor_
    ~ changeFriend("dor", 1)
    ~ friend++
    -> dor_main
* [Why work here, then?] Why would you want to work somewhere you're clearly not invested in? #arc_skept
    Hey, not everyone has the money to do whatever they want. #dor_conce
    I guess I didn't really think of that. Sorry. #arc_guilt
    ...It's okay. You're right though, I am trying to get out of here. #dor_conce
    -> dor_main
* [I'm going to tell your boss.] Frankly, I think I should tell your boss. Your service is clearly inadequate. #arc_disda
    Ugh. Yeah, okay, do whatever you want. I'm done with this job. #dor_conce
    ~ friend -= 2
    ~ changeFriend("dor", -2)
    -> dor_end
    
= dor_tumble
~ didsomething = true
And for that matter, what do topiaries have to do with airships? #arc_skept
I... I honestly have no idea. It's a little silly, isn't it? #dor_happy
I suppose I can't really complain, though. My mom named me 'Door'. #dor_conce
I like to pretend it's short for Dorothy. #dor_conce
* Door is a great name! #arc_happy
    ~ changeName("dor", "Door")
    You think so? #dor_embar
    Absolutely. #arc_happy
    ... #dor_embar
    I still kind of hate it, but maybe a bit less. Thanks. #dor_happy
    ~ changeFriend("dor", 1)
    ~ friend++
* Can't say I blame you. #arc_grin
    ~ changeName("dor", "Dorothy")
- -> dor_main

= dor_canned
Do you have to say that every time? #arc_skept
Unfortunately, yes. #dor_bored
-> dor_main

= dor_end
~ didsomething = false
^S
-> END

=== start ===
(SKIP LINE)
-> dor_at_lighthouse.dor_greet

