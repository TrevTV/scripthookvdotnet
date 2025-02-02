//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System.Drawing;

namespace GTA.UI
{
	/// <summary>
	/// Methods to handle UI actions that affect the whole screen.
	/// </summary>
	public static class Screen
	{
		#region Fields
		private static readonly string[] _effects = new string[] {
			"SwitchHUDIn",
			"SwitchHUDOut",
			"FocusIn",
			"FocusOut",
			"MinigameEndNeutral",
			"MinigameEndTrevor",
			"MinigameEndFranklin",
			"MinigameEndMichael",
			"MinigameTransitionOut",
			"MinigameTransitionIn",
			"SwitchShortNeutralIn",
			"SwitchShortFranklinIn",
			"SwitchShortTrevorIn",
			"SwitchShortMichaelIn",
			"SwitchOpenMichaelIn",
			"SwitchOpenFranklinIn",
			"SwitchOpenTrevorIn",
			"SwitchHUDMichaelOut",
			"SwitchHUDFranklinOut",
			"SwitchHUDTrevorOut",
			"SwitchShortFranklinMid",
			"SwitchShortMichaelMid",
			"SwitchShortTrevorMid",
			"DeathFailOut",
			"CamPushInNeutral",
			"CamPushInFranklin",
			"CamPushInMichael",
			"CamPushInTrevor",
			"SwitchSceneFranklin",
			"SwitchSceneTrevor",
			"SwitchSceneMichael",
			"SwitchSceneNeutral",
			"MP_Celeb_Win",
			"MP_Celeb_Win_Out",
			"MP_Celeb_Lose",
			"MP_Celeb_Lose_Out",
			"DeathFailNeutralIn",
			"DeathFailMPDark",
			"DeathFailMPIn",
			"MP_Celeb_Preload_Fade",
			"PeyoteEndOut",
			"PeyoteEndIn",
			"PeyoteIn",
			"PeyoteOut",
			"MP_race_crash",
			"SuccessFranklin",
			"SuccessTrevor",
			"SuccessMichael",
			"DrugsMichaelAliensFightIn",
			"DrugsMichaelAliensFight",
			"DrugsMichaelAliensFightOut",
			"DrugsTrevorClownsFightIn",
			"DrugsTrevorClownsFight",
			"DrugsTrevorClownsFightOut",
			"HeistCelebPass",
			"HeistCelebPassBW",
			"HeistCelebEnd",
			"HeistCelebToast",
			"MenuMGHeistIn",
			"MenuMGTournamentIn",
			"MenuMGSelectionIn",
			"ChopVision",
			"DMT_flight_intro",
			"DMT_flight",
			"DrugsDrivingIn",
			"DrugsDrivingOut",
			"SwitchOpenNeutralFIB5",
			"HeistLocate",
			"MP_job_load",
			"RaceTurbo",
			"MP_intro_logo",
			"HeistTripSkipFade",
			"MenuMGHeistOut",
			"MP_corona_switch",
			"MenuMGSelectionTint",
			"SuccessNeutral",
			"ExplosionJosh3",
			"SniperOverlay",
			"RampageOut",
			"Rampage",
			"Dont_tazeme_bro"
		};
		#endregion

		// Dimensions

		/// <summary>
		/// The base width of the screen used for all UI Calculations, unless ScaledDraw is used
		/// </summary>
		public const float Width = 1280f;
		/// <summary>
		/// The base height of the screen used for all UI Calculations
		/// </summary>
		public const float Height = 720f;

		/// <summary>
		/// Gets the actual screen resolution the game is being rendered at
		/// </summary>
		public static Size Resolution
		{
			get
			{
				int width, height;
				unsafe
				{
					Function.Call(Hash._GET_ACTIVE_SCREEN_RESOLUTION, &width, &height);
				}

				return new Size(width, height);
			}
		}
		/// <summary>
		/// Gets the current screen aspect ratio
		/// </summary>
		public static float AspectRatio => Function.Call<float>(Hash._GET_ASPECT_RATIO, 0);
		/// <summary>
		/// Gets the screen width scaled against a 720pixel height base.
		/// </summary>
		public static float ScaledWidth => Height * AspectRatio;

		// Fading

		/// <summary>
		/// Gets a value indicating whether the screen is faded in.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the screen is faded in; otherwise, <see langword="false" />.
		/// </value>
		public static bool IsFadedIn => Function.Call<bool>(Hash.IS_SCREEN_FADED_IN);
		/// <summary>
		/// Gets a value indicating whether the screen is faded out.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the screen is faded out; otherwise, <see langword="false" />.
		/// </value>
		public static bool IsFadedOut => Function.Call<bool>(Hash.IS_SCREEN_FADED_OUT);
		/// <summary>
		/// Gets a value indicating whether the screen is fading in.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the screen is fading in; otherwise, <see langword="false" />.
		/// </value>
		public static bool IsFadingIn => Function.Call<bool>(Hash.IS_SCREEN_FADING_IN);
		/// <summary>
		/// Gets a value indicating whether the screen is fading out.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the screen is fading out; otherwise, <see langword="false" />.
		/// </value>
		public static bool IsFadingOut => Function.Call<bool>(Hash.IS_SCREEN_FADING_OUT);

		/// <summary>
		/// Gets a value indicating whether screen kill effects are enabled.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if screen kill effects are enabled; otherwise, <see langword="false" />.
		/// </value>
		public static bool AreScreenKillEffectsEnabled => Game.GetProfileSetting(226) != 0;

		/// <summary>
		/// Fades the screen in over a specific time, useful for transitioning
		/// </summary>
		/// <param name="time">The time for the fade in to take</param>
		public static void FadeIn(int time)
		{
			Function.Call(Hash.DO_SCREEN_FADE_IN, time);
		}
		/// <summary>
		/// Fades the screen out over a specific time, useful for transitioning
		/// </summary>
		/// <param name="time">The time for the fade out to take</param>
		public static void FadeOut(int time)
		{
			Function.Call(Hash.DO_SCREEN_FADE_OUT, time);
		}

		// Screen Effects

		/// <summary>
		/// Gets a value indicating whether the specific screen effect is running.
		/// </summary>
		/// <param name="effectName">The <see cref="ScreenEffect"/> to check.</param>
		/// <returns><see langword="true" /> if the screen effect is active; otherwise, <see langword="false" />.</returns>
		public static bool IsEffectActive(ScreenEffect effectName)
		{
			return Function.Call<bool>(Hash.ANIMPOSTFX_IS_RUNNING, _effects[(int)effectName]);
		}

		/// <summary>
		/// Starts applying the specified effect to the screen.
		/// </summary>
		/// <param name="effectName">The <see cref="ScreenEffect"/> to start playing.</param>
		/// <param name="duration">The duration of the effect in milliseconds or zero to use the default length.</param>
		/// <param name="looped">If <see langword="true" /> the effect won't stop until <see cref="Screen.StopEffect(ScreenEffect)"/> is called.</param>
		public static void StartEffect(ScreenEffect effectName, int duration = 0, bool looped = false)
		{
			Function.Call(Hash.ANIMPOSTFX_PLAY, _effects[(int)effectName], duration, looped);
		}
		/// <summary>
		/// Stops applying the specified effect to the screen.
		/// </summary>
		/// <param name="effectName">The <see cref="ScreenEffect"/> to stop playing.</param>
		public static void StopEffect(ScreenEffect effectName)
		{
			Function.Call(Hash.ANIMPOSTFX_STOP, _effects[(int)effectName]);
		}
		/// <summary>
		/// Stops all currently running effects.
		/// </summary>
		public static void StopEffects()
		{
			Function.Call(Hash.ANIMPOSTFX_STOP_ALL);
		}

		// Text

		/// <summary>
		/// Shows a subtitle at the bottom of the screen for a given time
		/// </summary>
		/// <param name="message">The message to display.</param>
		/// <param name="duration">The duration to display the subtitle in milliseconds.</param>
		public static void ShowSubtitle(string message, int duration = 2500)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_PRINT, SHVDN.NativeMemory.CellEmailBcon);
			SHVDN.NativeFunc.PushLongString(message);
			Function.Call(Hash.END_TEXT_COMMAND_PRINT, duration, 1);
		}
		/// <summary>
		/// Displays a help message in the top corner of the screen this frame. Beeping sound will be played.
		/// </summary>
		/// <param name="helpText">The text to display.</param>
		public static void ShowHelpTextThisFrame(string helpText)
		{
			ShowHelpTextThisFrame(helpText, true);			// keeping it DRY :)
		}
		/// <summary>
		/// Displays a help message in the top corner of the screen this frame. Specify whether beeping sound plays.
		/// </summary>
		/// <param name="helpText">The text to display.</param>
		/// <param name="beep">Whether to play beeping sound</param>
		public static void ShowHelpTextThisFrame(string helpText, bool beep)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, SHVDN.NativeMemory.CellEmailBcon);
			SHVDN.NativeFunc.PushLongString(helpText);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, beep, -1);
		}


		// Space Conversion

		/// <summary>
		/// Translates a point in WorldSpace to its given Coordinates on the <see cref="Screen"/>
		/// </summary>
		/// <param name="position">The position in the World.</param>
		/// <param name="scaleWidth">if set to <see langword="true" /> Returns the screen position scaled by <see cref="ScaledWidth"/>; otherwise, returns the screen position scaled by <see cref="Width"/>.</param>
		/// <returns></returns>
		public static PointF WorldToScreen(Vector3 position, bool scaleWidth = false)
		{
			float pointX, pointY;

			unsafe
			{
				if (!Function.Call<bool>(Hash.GET_SCREEN_COORD_FROM_WORLD_COORD, position.X, position.Y, position.Z, &pointX, &pointY))
				{
					return PointF.Empty;
				}
			}

			pointX *= scaleWidth ? ScaledWidth : Width;
			pointY *= Height;

			return new PointF(pointX, pointY);
		}
	}
}
