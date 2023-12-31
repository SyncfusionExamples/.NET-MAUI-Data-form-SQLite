﻿using Syncfusion.Maui.DataForm;

namespace DataFormMAUI
{

    /// <summary>
    /// Represents a behavior class for contact form.
    /// </summary>
    public class ContactFormBehavior : Behavior<ContentPage>
    {
        /// <summary>
        /// Holds the data form object.
        /// </summary>
        private SfDataForm? dataForm;

        private bool isValid;

        /// <summary>
        /// Holds the save button instance.
        /// </summary>
        private Button? saveButton;

        /// <inheritdoc/>
        protected override void OnAttachedTo(ContentPage bindable)
        {
            base.OnAttachedTo(bindable);
            this.dataForm = bindable.FindByName<SfDataForm>("contactForm");

            if (this.dataForm != null)
            {
                dataForm.ValidateForm += this.OnDataFormValidateForm;
                dataForm.ValidateProperty += this.OnDataFormValidateProperty;
            }

            this.saveButton = bindable.FindByName<Button>("saveButton");
            if (this.saveButton != null)
            {
                this.saveButton.Clicked += OnSaveButtonClicked;
            }
        }
     
        /// <summary>
        /// Invokes on data form item validation.
        /// </summary>
        /// <param name="sender">The data form.</param>
        /// <param name="e">The event arguments.</param>
        private void OnDataFormValidateProperty(object? sender, DataFormValidatePropertyEventArgs e)
        {
            if (e.PropertyName == nameof(ContactFormModel.Mobile) && !e.IsValid)
            {
                e.ErrorMessage = e.NewValue == null || string.IsNullOrEmpty(e.NewValue.ToString()) ? "Please enter the mobile number" : "Invalid mobile number";
            }
        }

        /// <summary>
        /// Invokes on manual data form validation.
        /// </summary>
        /// <param name="sender">The data form.</param>
        /// <param name="e">The event arguments.</param>
        private async void OnDataFormValidateForm(object? sender, DataFormValidateFormEventArgs e)
        {
            if (this.dataForm != null && App.Current?.MainPage != null)
            {
                if (e.ErrorMessage != null && e.ErrorMessage.Count > 0)
                {
                    if (e.ErrorMessage.Count == 2)
                    {
                        await App.Current.MainPage.DisplayAlert("", "Please enter the contact name and mobile number", "OK");
                    }
                    else
                    {
                        if (e.ErrorMessage.ContainsKey(nameof(ContactFormModel.Name)))
                        {
                            await App.Current.MainPage.DisplayAlert("", "Please enter the contact name", "OK");
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("", "Please enter the mobile number", "OK");
                        }
                    }
                    isValid = false;

                }
                else
                {
                    isValid = true;
               
                }
            }
        }

        /// <summary>
        /// Invokes on save button click.
        /// </summary>
        /// <param name="sender">The button.</param>
        /// <param name="e">The event arguments.</param>
        private async void OnSaveButtonClicked(object? sender, EventArgs e)
        {
            this.dataForm?.Validate();

            if(isValid)
            {
                await Task.Delay(500);
                await App.Current.MainPage.Navigation.PopAsync();
                await App.Current.MainPage.DisplayAlert("", "Contact saved", "OK");

            }
        }

        /// <inheritdoc/>
        protected override void OnDetachingFrom(ContentPage bindable)
        {
            base.OnDetachingFrom(bindable);
            if (this.dataForm != null)
            {
                this.dataForm.ValidateForm -= this.OnDataFormValidateForm;
                this.dataForm.ValidateProperty -= this.OnDataFormValidateProperty;
                this.dataForm = null;
            }

            if (this.saveButton != null)
            {
                this.saveButton.Clicked -= OnSaveButtonClicked;
                this.saveButton = null;
            }
        }
    }
}