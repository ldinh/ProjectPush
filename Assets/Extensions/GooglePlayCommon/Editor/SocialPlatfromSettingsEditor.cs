
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(SocialPlatfromSettings))]
public class SocialPlatfromSettingsEditor : Editor {


	

	static GUIContent TConsumerKey   = new GUIContent("Consumer Key [?]:", "Twitter register app consumer key");
	static GUIContent TConsumerSecret   = new GUIContent("Consumer Secret [?]:", "Twitter register app consumer secret");


	
	static GUIContent SdkVersion   = new GUIContent("Plugin Version [?]", "This is Plugin version.  If you have problems or compliments please include this so we know exactly what version to look out for.");
	static GUIContent SupportEmail = new GUIContent("Support [?]", "If you have any technical quastion, feel free to drop an e-mail");



	private const string IOS_SOURCE_PATH 			= "Plugins/StansAssets/IOS/";
	private const string IOS_DESTANATION_PATH 		= "Plugins/IOS/";
	private const string ANDROID_SOURCE_PATH 		= "Plugins/StansAssets/Android/";
	private const string ANDROID_DESTANATION_PATH 	= "Plugins/Android/";

	private const string version_info_file = "Plugins/StansAssets/Versions/MSP_VersionInfo.txt"; 
	

	public override void OnInspectorGUI() {


		GUI.changed = false;


		if(IsFullVersion) {
			GeneralOptions();
			EditorGUILayout.Space();
		}

		FacebookSettings();
		EditorGUILayout.Space();
		TwitterSettings();
		EditorGUILayout.Space();

		AboutGUI();
	



		if(GUI.changed) {
			DirtyEditor();
		}
	}


	public static bool IsFullVersion {
		get {
			if(FileStaticAPI.IsFileExists(PluginsInstalationUtil.IOS_SOURCE_PATH + "MGInstagram.h")) {
				return true;
			} else {
				return false;
			}
		}
	}
	

	public static bool IsInstalled {
		get {
			if(FileStaticAPI.IsFileExists(PluginsInstalationUtil.ANDROID_DESTANATION_PATH + "androidnative.jar") && FileStaticAPI.IsFileExists(PluginsInstalationUtil.IOS_DESTANATION_PATH + "MGInstagram.h")) {
				return true;
			} else {
				return false;
			}
		}
	}
	
	public static bool IsUpToDate {
		get {
			if(SocialPlatfromSettings.VERSION_NUMBER.Equals(DataVersion)) {
				return true;
			} else {
				return false;
			}
		}
	}
	
	
	public static string DataVersion {
		get {
			if(FileStaticAPI.IsFileExists(version_info_file)) {
				return FileStaticAPI.Read(version_info_file);
			} else {
				return "Unknown";
			}
		}
	}



	private void GeneralOptions() {
		
		
		
		if(!IsInstalled) {
			EditorGUILayout.HelpBox("Install Required ", MessageType.Error);
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			Color c = GUI.color;
			GUI.color = Color.cyan;
			if(GUILayout.Button("Install Plugin",  GUILayout.Width(120))) {
				PluginsInstalationUtil.Android_InstallPlugin();
				PluginsInstalationUtil.IOS_InstallPlugin();
				UpdateVersionInfo();
			}
			GUI.color = c;
			EditorGUILayout.EndHorizontal();
		}
		
		if(IsInstalled) {
			if(!IsUpToDate) {
				EditorGUILayout.HelpBox("Update Required \nResources version: " + DataVersion + " Plugin version: " + SocialPlatfromSettings.VERSION_NUMBER, MessageType.Warning);
				
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				Color c = GUI.color;
				GUI.color = Color.cyan;
				if(GUILayout.Button("Update to " + SocialPlatfromSettings.VERSION_NUMBER,  GUILayout.Width(250))) {
					PluginsInstalationUtil.Android_InstallPlugin();
					PluginsInstalationUtil.IOS_InstallPlugin();
					UpdateVersionInfo();
				}
				
				GUI.color = c;
				EditorGUILayout.Space();
				EditorGUILayout.EndHorizontal();
				
			} else {
				EditorGUILayout.HelpBox("Mobile Social Plugin v" + SocialPlatfromSettings.VERSION_NUMBER + " is installed", MessageType.Info);

			}
		}
		
		
		EditorGUILayout.Space();
		
	}


	private static string newPermition = "";
	public static void FacebookSettings() {
		EditorGUILayout.HelpBox("Facebook Settings", MessageType.None);

		SocialPlatfromSettings.Instance.showPermitions = EditorGUILayout.Foldout(SocialPlatfromSettings.Instance.showPermitions, "Permissions");
		if(SocialPlatfromSettings.Instance.showPermitions) {
			foreach(string s in SocialPlatfromSettings.Instance.fb_scopes_list) {
				EditorGUILayout.BeginVertical (GUI.skin.box);
				EditorGUILayout.BeginHorizontal();

				EditorGUILayout.SelectableLabel(s, GUILayout.Height(16));
				
				if(GUILayout.Button("x",  GUILayout.Width(20))) {
					SocialPlatfromSettings.Instance.fb_scopes_list.Remove(s);
					return;
				}
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.EndVertical();
			}
			
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Add new permition: ");
			newPermition = EditorGUILayout.TextField(newPermition);
			
			
			EditorGUILayout.EndHorizontal();
			
			
			
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.Space();
			if(GUILayout.Button("Documentation",  GUILayout.Width(100))) {
				Application.OpenURL("https://developers.facebook.com/docs/facebook-login/permissions/v2.0");
			}
			
			
			
			if(GUILayout.Button("Add",  GUILayout.Width(100))) {
				
				if(newPermition != string.Empty) {
					if(!SocialPlatfromSettings.Instance.fb_scopes_list.Contains(newPermition)) {
						SocialPlatfromSettings.Instance.fb_scopes_list.Add(newPermition);
					}
					
					newPermition = string.Empty;
				}
			}
			EditorGUILayout.EndHorizontal();
		
		}

	}

	public static void TwitterSettings() {
		EditorGUILayout.HelpBox("Twitter Settings", MessageType.None);


		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(TConsumerKey);
		SocialPlatfromSettings.Instance.TWITTER_CONSUMER_KEY	 	= EditorGUILayout.TextField(SocialPlatfromSettings.Instance.TWITTER_CONSUMER_KEY);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(TConsumerSecret);
		SocialPlatfromSettings.Instance.TWITTER_CONSUMER_SECRET	 	= EditorGUILayout.TextField(SocialPlatfromSettings.Instance.TWITTER_CONSUMER_SECRET);
		EditorGUILayout.EndHorizontal();
	}




	private void AboutGUI() {


		EditorGUILayout.HelpBox("About Mobile Social Plugin", MessageType.None);
		EditorGUILayout.Space();
		
		SelectableLabelField(SdkVersion, SocialPlatfromSettings.VERSION_NUMBER);
		SelectableLabelField(SupportEmail, "stans.assets@gmail.com");

		
	}
	
	private static void SelectableLabelField(GUIContent label, string value) {
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(label, GUILayout.Width(180), GUILayout.Height(16));
		EditorGUILayout.SelectableLabel(value, GUILayout.Height(16));
		EditorGUILayout.EndHorizontal();
	}

	public static void UpdateVersionInfo() {
		FileStaticAPI.Write(version_info_file, SocialPlatfromSettings.VERSION_NUMBER);
	}



	public static void DirtyEditor() {
		#if UNITY_EDITOR
		EditorUtility.SetDirty(SocialPlatfromSettings.Instance);
		#endif
	}
	
	
}
