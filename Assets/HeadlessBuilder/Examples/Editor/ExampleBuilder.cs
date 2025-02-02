/* 
 * Headless Builder
 * (c) Salty Devs, 2020
 * 
 * Please do not publish or pirate this code.
 * We worked really hard to make it.
 * 
 */

using UnityEditor;

// This class provides example callbacks for Headless Builder during building
[HeadlessCallbacks]
public static class ExampleBuilder {

	public static void HeadlessBuildBefore() {
		// This code will be executed before making an headless build
		//Debug.Log ("The build is about to start.");
	}

	public static void HeadlessBuildSuccess() {
		// This code will be executed after succesfully making an headless build
		//Debug.Log ("The build just finished.");
	}

	public static void HeadlessBuildFailed() {
		// This code will be executed after failing to make an headless build
		//Debug.Log ("The build just failed.");
	}

	public static BuildOptions HeadlessBuildOptions(BuildOptions buildOptions) {
		// This code will be executed before making an headless build
		// and allows you to modify the BuildOptions.
		//Debug.Log ("The build options were modified by a callback.");
		return buildOptions;
	}

#if UNITY_2021_2_OR_NEWER
	public static BuildPlayerOptions HeadlessBuildPlayerOptions(BuildPlayerOptions buildPlayerOptions) {
		// This code will be executed before making an headless build (on Unity 2021.2 and newer)
		// and allows you to modify the BuildPlayerOptions.
		//Debug.Log ("The build player options were modified by a callback.");
		return buildPlayerOptions;
	}
#endif

}
