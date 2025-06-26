#luc_0
(SKIPPED)

EXTERNAL openShop(name)

Hey, can I get anything for ya? #luc_tired
~ openShop("LucyShop")
-> idle

=== idle ===
Sorry we're low on stock - you came at a bad time.^P^B #luc_tired
Most of the town has been out on a big fishing trip for the past week.^P^B #luc_tired
Truth is, it's been harder and harder to find fish...^P Nobody's really sure why, but I think -^1^B #luc_tired
Steven! Let your brother go!^P^B #luc_angry
Sigh... kids. #luc_tired
-> DONE

=== purchase ===
Wonderful! Thanks for stopping by. #luc_happy
-> generic

=== too_expensive ===
Ah! Sorry, I won't part with it for less. Money's tight. #luc_
-> generic

=== compass ===
A compass? We've got tons of these. They're handy for navigating the open ocean, but I'm sure you'll find your own use for it. #luc_happy
-> generic

=== seashell ===
These seashells are very popular over in the city, but they're pretty common around here. #luc_happy
-> generic

=== haddock ===
The whole town's low on fish right now, but I'll get you what I can. #luc_happy
-> generic

=== generic ===
Can I help you with anything else? #luc_tired
-> generic

=== exit ===
Take care, then! #luc_happy
^S
-> END