#gom_shop

(SKIP LINE)

VAR didSomething = false

EXTERNAL openShop(name)

hmm... something smells... #gom_sus
....rotten....mmmmm.... #gom_
has one brought presents for gom? #gom_
~ openShop("GomShop")
-> generic

=== generic ===
behold... the precious treasures of gom... #gom_
-> generic

=== purchase ===
~ didSomething = true
mmm... an excellent choice. #gom_
-> generic

=== too_expensive ===
mmm... gom's treasures are worth more than one offers. #gom_sus
-> generic

=== a_friend ===
~ didSomething = true
{ stopping:
    - one always has need of a friend, no? gom has found this. #gom_sus
    - gom believes the friend has taken a liking to one already. #gom_
}
-> generic

=== quality_propeller ===
~ didSomething = true
mmm... one has a good eye. is this not the finest treasure gom has ever possessed? #gom_shock
-> generic

=== blessing ===
~ didSomething = true
secrets of great power, knows gom. with such a blessing, one might ward off great evils. #gom_
-> generic

=== exit ===
{
    - not didSomething: impossible! the wares of gom, do they not interest one? #gom_shock
        gom is ashamed. #gom_
    - gom is thankful for one's patronage. #gom_
}
^S -> END