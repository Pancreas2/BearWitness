#lighthouse_unlock

EXTERNAL hasKey(name)
EXTERNAL openDoor(name)
-> main

=== main ===
An old wooden door. #arc_
{
- hasKey("Eyestone"): It's locked. #arc_conce
    -> use_key 
- It's locked. #arc_conce
    And the statue on the arch seems to be ^CYmissing an eye^CX. #arc_skept
    -> end
}

=== use_key ===
+ [Use the Eyestone] It fits perfectly! #arc_happy
    ~ openDoor("Lighthouse")
    -> end
+ [Do not] I think I'll hold on to it. #arc_
    -> end

=== end ===
^S
-> END

=== start ===
-> main
