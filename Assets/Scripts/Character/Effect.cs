using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Effect{

    public enum effect { Slow, Stun };

    public static void doEffect(List<effect> effects, Character target)
    {
        foreach(effect whatEffect in effects){
            FindEffect(whatEffect, target);
        }
    }

    private static void FindEffect(effect whatEffect, Character character)
    {
        switch (whatEffect) {
            case effect.Slow:
                Slow(character);
                break;
            case effect.Stun:
                Stun(character);
                break;
        }
    }

    private static void Stun(Character character)
    {
        Debug.Log("STUN - NOT IMPLEMENTED");
    }

    private static void Slow(Character character)
    {
        Debug.Log("SLOW - NOT IMPLEMENTED");
    }
}

