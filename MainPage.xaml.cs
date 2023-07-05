using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Plugin.Firebase.CloudMessaging;

namespace PushMauiSample;

public partial class MainPage : ContentPage
{
	public string Token { get; set; }
	
	public MainPage()
	{
		InitializeComponent();
	}

	private async void GetTokenClicked(object sender, EventArgs e)
	{
		await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
		var token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
		Token = token;
		await DisplayAlert("FCM token", token, "OK");
	}
	
	private async void SendPushClicked(object sender, EventArgs e)
	{
		var app = FirebaseApp.Create(new AppOptions
		{
			Credential = await GetCredential()
		});
		
		FirebaseMessaging messaging = FirebaseMessaging.GetMessaging(app);
		var message = new Message()
		{
			Token = Token,
			Notification = new Notification { Title = "Hello bro!", Body = "It's a message for Android with MAUI"},
			Data = new Dictionary<string, string> {{"greating", "hello"}},
			Android = new AndroidConfig { Priority = Priority.Normal},
			Apns = new ApnsConfig { Headers = new Dictionary<string, string> { {"apns-priority", "5"}}}
		};
		var response = await messaging.SendAsync(message);
		await DisplayAlert("Response", response, "OK");
	}
	
	private async Task<GoogleCredential> GetCredential()
	{
		var path = await FileSystem.OpenAppPackageFileAsync("firebase-adminsdk.json");
		return GoogleCredential.FromStream(path);
	}
}

