using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;

public class NeeDeezNuts : MonoBehaviour {
   public KMAudio Audio;
   public KMBombInfo Bomb;
   public KMNeedyModule Module;
   public KMSelectable[] Buttons;
   public TextMesh[] ButtonTM;

   private bool active;
   private static int moduleIdCounter = 1;
   private int moduleId;
   private bool moduleSolved;
   bool[] HasDeezNuts = { false, false, false, false };

   void Awake () {
      moduleId = moduleIdCounter++;
      Module.OnNeedyActivation += OnNeedyActivation;
      Module.OnNeedyDeactivation += OnNeedyDeactivation;
      Module.OnTimerExpired += OnTimerExpired;
      foreach (KMSelectable Button in Buttons) {
         Button.OnInteract += delegate () { ButtonPress(Button); return false; };
      }
   }

   void Start () {
      Module.SetResetDelayTime(30f, 50f);
   }

   void ButtonPress (KMSelectable Button) {
      Button.AddInteractionPunch();
      Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Button.transform);
      for (int i = 0; i < 4; i++) {
         if (Button == Buttons[i] && HasDeezNuts[i]) {
            Debug.LogFormat("[NeeDeez Nuts #{0}] Deez nuts!", moduleId);
            Audio.PlaySoundAtTransform("DeezNuts", transform);
            OnNeedyDeactivation();
            Module.OnPass();
            Button.AddInteractionPunch();
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Button.transform);
            for (int j = 0; j < 4; j++) {
               HasDeezNuts[j] = false;
               ButtonTM[j].text = "Got Eeem!";
            }
         }
         else if (Button == Buttons[i] && !HasDeezNuts[i]) {
            Debug.LogFormat("[NeeDeez Nuts #{0}] Got Eeem!", moduleId);
            Audio.PlaySoundAtTransform("GOTEEE", transform);
            Module.OnStrike();
            Button.AddInteractionPunch();
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Button.transform);
         }
      }
   }

   protected void OnNeedyActivation () {
      active = true;
      int temp = Rnd.Range(0, 4);
      HasDeezNuts[temp] = true;
      ButtonTM[temp].text = "Deez Nuts!";
      for (int i = 0; i < 4; i++) {
         if (i != temp) {
            ButtonTM[i].text = "Got Eeem!";
         }
      }
   }

   protected void OnNeedyDeactivation () {
      active = false;
   }

   protected void OnTimerExpired () {
      if (active) {
         Module.OnStrike();
         OnNeedyDeactivation();
      }
   }

   //I add the twitch plays

#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"!{0} press TL/TR/BL/BR to press the corresponding button.";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand (string command) {
      command = command.Trim();
      string[] parameters = command.Split(' ');
      if (parameters.Length > 2) {
         yield return null;
         yield return "sendtochaterror Too many words";
         yield break;
      }
      else if (parameters.Length == 2) {
         yield return null;
         if (parameters[0].ToUpper() == "PRESS") {
            if (parameters[1].ToUpper() == "TL") {
               Buttons[0].OnInteract();
            }
            else if (parameters[1].ToUpper() == "TR") {
               Buttons[1].OnInteract();
            }
            else if (parameters[1].ToUpper() == "BL") {
               Buttons[3].OnInteract();
            }
            else if (parameters[1].ToUpper() == "BR") {
               Buttons[2].OnInteract();
            }
            else {
               yield return "sendtochaterror Invalid command.";
               yield break;
            }
         }
         else {
            yield return null;
            yield return "sendtochaterror Invalid command.";
            yield break;
         }
      }
      if (parameters.Length < 2) {
         yield return null;
         yield return "sendtochaterror Too little words";
         yield break;
      }
   }
}
