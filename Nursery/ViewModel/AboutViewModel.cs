using System.Windows;


namespace Nursery.ViewModel
{
    internal class AboutViewModel : BaseViewModel
    {

        public AboutViewModel(Window window)
        {
            this.window = window;

        }
        //public string WelcomeText
        //{
        //    get
        //    {
        //        return $"Здравствуйте! Вас приветствует компания ООО Ми-Ми-ми. Данная программа была разработана специально для нашей компании и все права на её использование принадлежат нам.";
        //    }
        //}

        public RealiseCommand CloseWindow
        {
            get
            {
                return new RealiseCommand((obj) =>
                {

                    window.Close();
                });
            }
        }

    }
}
