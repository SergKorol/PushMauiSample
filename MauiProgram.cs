using Microsoft.Maui.LifecycleEvents;
using Plugin.Firebase.Android;
using Plugin.Firebase.Auth;
// using Plugin.Firebase.Bundled.Platforms.Android;
// using Plugin.Firebase.Bundled.Shared;
using Plugin.Firebase.Shared;


namespace PushMauiSample;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.RegisterFirebaseServices()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		return builder.Build();
	}
	
	private static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
	{
		builder.ConfigureLifecycleEvents(events =>
		{

			events.AddAndroid(android => android.OnCreate((activity, state) =>
				CrossFirebase.Initialize(activity, state, CreateCrossFirebaseSettings())));
		});

		builder.Services.AddSingleton(_ => CrossFirebaseAuth.Current);
		return builder;
	}

	private static CrossFirebaseSettings CreateCrossFirebaseSettings()
	{
		return new CrossFirebaseSettings(isAuthEnabled: true, isCloudMessagingEnabled: true);
	}
}
