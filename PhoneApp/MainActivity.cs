using Android.App;
using Android.Widget;
using Android.OS;
using SALLab05;
using System.Collections.Generic;

namespace PhoneApp
{
    [Activity(Label = "Phone App", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        static readonly List<string> phoneNumbers = new List<string>();
        TextView MessageText;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            var PhoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
            var TranslateButton = FindViewById<Button>(Resource.Id.TranslateButton);
            var CallButton = FindViewById<Button>(Resource.Id.CallButton);
            //var CallHistoryButton = FindViewById<Button>(Resource.Id.CallHistoryButton);
            MessageText = FindViewById<TextView>(Resource.Id.MessageText);

            CallButton.Enabled = false;

            var TranslatedNumber = string.Empty;

            TranslateButton.Click += (object sender, System.EventArgs e) =>
            {
                var Translator = new PhoneTranslator();
                TranslatedNumber = Translator.ToNumber(PhoneNumberText.Text);
                if (string.IsNullOrWhiteSpace(TranslatedNumber))
                {
                    // No hay número a llamar
                    CallButton.Text = "Llamar";
                    CallButton.Enabled = false;
                }
                else
                {
                    CallButton.Enabled = true;
                }
            };

            CallButton.Click += (object sender, System.EventArgs e) =>
            {
                // Intentar marcar el número telefónico
                var CallDialog = new AlertDialog.Builder(this);
                CallDialog.SetMessage($"Llamar al número {TranslatedNumber}?");
                CallDialog.SetNeutralButton("Llamar", delegate
                {
                    // Agregar el número marcado a la lista de números marcados
                    phoneNumbers.Add(TranslatedNumber);

                    // Habilitar botón CallHistotyButton
                    //CallHistoryButton.Enabled = true;

                    // Crear un intento para marcar el número telefónico
                    var CallIntent =
                       new Android.Content.Intent(Android.Content.Intent.ActionCall);
                    CallIntent.SetData(
                        Android.Net.Uri.Parse($"tel:{TranslatedNumber}"));
                    StartActivity(CallIntent);
                });

                CallDialog.SetNegativeButton("Cancelar", delegate { });
                // Mostrar el cuadro de diálogo al usuario y esperar una respuesta.
                CallDialog.Show();
            };

            //CallHistoryButton.Click += (sender, e) =>
            //{
            //    var intent = new Android.Content.Intent(this, typeof(CallHistoryActivity));
            //    intent.PutStringArrayListExtra("phone_numbers", phoneNumbers);
            //    StartActivity(intent);
            //};

            Validate();
        }

        private async void Validate()
        {
            var serviceClient = new ServiceClient();
            var studentEmail = "jzuluaga55@gmail.com";
            var password = "Roger1974";
            var myDevice = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId);
            var result = await serviceClient.ValidateAsync(studentEmail, password, myDevice);
            MessageText.Text = $"{result.Status}\n{result.Fullname}\n{result.Token}";
            //var builder = new AlertDialog.Builder(this);
            //var alert = builder.Create();
            //alert.SetTitle("Resultado de la verificación");
            //alert.SetIcon(Resource.Drawable.Icon);
            //alert.SetMessage($"{result.Status}\n{result.Fullname}\n{result.Token}");
            //alert.SetButton("Ok", (s, ev) => { });
            //alert.Show();
        }
    }
}