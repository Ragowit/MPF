﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MPF.Data;

namespace MPF.Windows
{
    /// <summary>
    /// Interaction logic for DiscInformationWindow.xaml
    /// </summary>
    public partial class DiscInformationWindow : Window
    {
        #region Fields

        /// <summary>
        /// List of available disc categories
        /// </summary>
        public List<Element<RedumpDiscCategory>> Categories { get; private set; } = Element<RedumpDiscCategory>.GenerateElements().ToList();

        /// <summary>
        /// SubmissionInfo object to fill and save
        /// </summary>
        public SubmissionInfo SubmissionInfo { get; set; }

        /// <summary>
        /// List of available regions
        /// </summary>
        public List<Element<RedumpRegion>> Regions { get; private set; } = Element<RedumpRegion>.GenerateElements().ToList();

        /// <summary>
        /// List of available languages
        /// </summary>
        public List<Element<RedumpLanguage>> Languages { get; private set; } = Element<RedumpLanguage>.GenerateElements().ToList();

        /// <summary>
        /// List of available languages
        /// </summary>
        public List<Element<RedumpLanguageSelection>> LanguageSelections { get; private set; } = Element<RedumpLanguageSelection>.GenerateElements().ToList();

        #endregion

        public DiscInformationWindow(SubmissionInfo submissionInfo)
        {
            InitializeComponent();
            DataContext = this;

            this.SubmissionInfo = submissionInfo;
            ManipulateFields();
        }

        /// <summary>
        /// Manipulate fields based on the current disc
        /// </summary>
        private void ManipulateFields()
        {
            // Different media types mean different fields available
            switch (SubmissionInfo?.CommonDiscInfo?.Media)
            {
                case MediaType.CDROM:
                case MediaType.GDROM:
                    L0Info.Header = "Data Side";
                    L0MasteringRing.Label = "Mastering Ring";
                    L0MasteringSID.Label = "Mastering SID";
                    L0Toolstamp.Label = "Toolstamp/Mastering Code";
                    L0MouldSID.Label = "Mould SID";
                    L0AdditionalMould.Label = "Additional Mould";

                    L1Info.Header = "Label Side";
                    L1MasteringRing.Label = "Mastering Ring";
                    L1MasteringSID.Label = "Mastering SID";
                    L1Toolstamp.Label = "Toolstamp/Mastering Code";
                    L1MouldSID.Label = "Mould SID";
                    L1AdditionalMould.Label = "Additional Mould";
                    break;

                case MediaType.DVD:
                case MediaType.HDDVD:
                case MediaType.BluRay:
                case MediaType.NintendoGameCubeGameDisc:
                case MediaType.NintendoWiiOpticalDisc:
                case MediaType.NintendoWiiUOpticalDisc:
                    // Quad-layer discs
                    if (SubmissionInfo?.SizeAndChecksums?.Layerbreak3 != default(long))
                    {
                        L2Info.Visibility = Visibility.Visible;
                        L3Info.Visibility = Visibility.Visible;

                        L0Info.Header = "Layer 0 (Inner)";
                        L0MasteringRing.Label = "Mastering Ring";
                        L0MasteringSID.Label = "Mastering SID";
                        L0Toolstamp.Label = "Toolstamp/Mastering Code";
                        L0MouldSID.Label = "Data-Side Mould SID";
                        L0AdditionalMould.Label = "Data-Side Additional Mould";

                        L1Info.Header = "Layer 1";
                        L1MasteringRing.Label = "Mastering Ring";
                        L1MasteringSID.Label = "Layer 1 Mastering SID";
                        L1Toolstamp.Label = "Toolstamp/Mastering Code";
                        L1MouldSID.Label = "Label-Side Mould SID";
                        L1AdditionalMould.Label = "Label-Side Additional Mould";

                        L2Info.Header = "Layer 2";
                        L2MasteringRing.Label = "Mastering Ring";
                        L2MasteringSID.Label = "Mastering SID";
                        L2Toolstamp.Label = "Toolstamp/Mastering Code";

                        L3Info.Header = "Layer 3 (Outer)";
                        L3MasteringRing.Label = "Mastering Ring";
                        L3MasteringSID.Label = "Mastering SID";
                        L3Toolstamp.Label = "Toolstamp/Mastering Code";
                    }

                    // Triple-layer discs
                    else if (SubmissionInfo?.SizeAndChecksums?.Layerbreak2 != default(long))
                    {
                        L2Info.Visibility = Visibility.Visible;

                        L0Info.Header = "Layer 0 (Inner)";
                        L0MasteringRing.Label = "Mastering Ring";
                        L0MasteringSID.Label = "Mastering SID";
                        L0Toolstamp.Label = "Toolstamp/Mastering Code";
                        L0MouldSID.Label = "Data-Side Mould SID";
                        L0AdditionalMould.Label = "Data-Side Additional Mould";

                        L1Info.Header = "Layer 1";
                        L1MasteringRing.Label = "Mastering Ring";
                        L1MasteringSID.Label = "Mastering SID";
                        L1Toolstamp.Label = "Toolstamp/Mastering Code";
                        L1MouldSID.Label = "Label-Side Mould SID";
                        L1AdditionalMould.Label = "Label-Side Additional Mould";

                        L2Info.Header = "Layer 2 (Outer)";
                        L2MasteringRing.Label = "Mastering Ring";
                        L2MasteringSID.Label = "Mastering SID";
                        L2Toolstamp.Label = "Toolstamp/Mastering Code";
                    }

                    // Double-layer discs
                    else if (SubmissionInfo?.SizeAndChecksums?.Layerbreak != default(long))
                    {
                        L0Info.Header = "Layer 0 (Inner)";
                        L0MasteringRing.Label = "Mastering Ring";
                        L0MasteringSID.Label = "Mastering SID";
                        L0Toolstamp.Label = "Toolstamp/Mastering Code";
                        L0MouldSID.Label = "Data-Side Mould SID";
                        L0AdditionalMould.Label = "Data-Side Additional Mould";

                        L1Info.Header = "Layer 1 (Outer)";
                        L1MasteringRing.Label = "Mastering Ring";
                        L1MasteringSID.Label = "Mastering SID";
                        L1Toolstamp.Label = "Toolstamp/Mastering Code";
                        L1MouldSID.Label = "Label-Side Mould SID";
                        L1AdditionalMould.Label = "Label-Side Additional Mould";
                    }

                    // Single-layer discs
                    else
                    {
                        L0Info.Header = "Data Side";
                        L0MasteringRing.Label = "Mastering Ring";
                        L0MasteringSID.Label = "Mastering SID";
                        L0Toolstamp.Label = "Toolstamp/Mastering Code";
                        L0MouldSID.Label = "Mould SID";
                        L0AdditionalMould.Label = "Additional Mould";

                        L1Info.Header = "Label Side";
                        L1MasteringRing.Label = "Mastering Ring";
                        L1MasteringSID.Label = "Mastering SID";
                        L1Toolstamp.Label = "Toolstamp/Mastering Code";
                        L1MouldSID.Label = "Mould SID";
                        L1AdditionalMould.Label = "Additional Mould";
                    }
                    
                    break;
            }
        
            // Different systems mean different fields available
            switch (SubmissionInfo?.CommonDiscInfo?.System)
            {
                case KnownSystem.SonyPlayStation2:
                    LanguageSelectionGrid.Visibility = Visibility.Visible;
                    break;
            }
        }

        /// <summary>
        /// Load the current contents of the base SubmissionInfo to the UI
        /// </summary>
        public void Load()
        {
            GameTitle.Text = SubmissionInfo.CommonDiscInfo.Title ?? "";
            ForeignTitle.Text = SubmissionInfo.CommonDiscInfo.ForeignTitleNonLatin ?? "";
            DiscNumberLetter.Text = SubmissionInfo.CommonDiscInfo.DiscNumberLetter ?? "";
            DiscTitle.Text = SubmissionInfo.CommonDiscInfo.DiscTitle ?? "";
            CategoryComboBox.SelectedIndex = Categories.FindIndex(r => r == SubmissionInfo.CommonDiscInfo.Category);
            RegionComboBox.SelectedIndex = Regions.FindIndex(r => r == SubmissionInfo.CommonDiscInfo.Region);
            if (SubmissionInfo.CommonDiscInfo.Languages != null)
                Languages.ForEach(l => l.IsChecked = SubmissionInfo.CommonDiscInfo.Languages.Contains(l));
            if (SubmissionInfo.CommonDiscInfo.LanguageSelection != null)
                LanguageSelections.ForEach(ls => ls.IsChecked = SubmissionInfo.CommonDiscInfo.LanguageSelection.Contains(ls));
            Serial.Text = SubmissionInfo.CommonDiscInfo.Serial ?? "";

            L0MasteringRing.Text = SubmissionInfo.CommonDiscInfo.Layer0MasteringRing ?? "";
            L0MasteringSID.Text = SubmissionInfo.CommonDiscInfo.Layer0MasteringSID ?? "";
            L0Toolstamp.Text = SubmissionInfo.CommonDiscInfo.Layer0ToolstampMasteringCode ?? "";
            L0MouldSID.Text = SubmissionInfo.CommonDiscInfo.Layer0MouldSID ?? "";
            L0AdditionalMould.Text = SubmissionInfo.CommonDiscInfo.Layer0AdditionalMould ?? "";

            L1MasteringRing.Text = SubmissionInfo.CommonDiscInfo.Layer1MasteringRing ?? "";
            L1MasteringSID.Text = SubmissionInfo.CommonDiscInfo.Layer1MasteringSID ?? "";
            L1Toolstamp.Text = SubmissionInfo.CommonDiscInfo.Layer1ToolstampMasteringCode ?? "";
            L1MouldSID.Text = SubmissionInfo.CommonDiscInfo.Layer1MouldSID ?? "";
            L1AdditionalMould.Text = SubmissionInfo.CommonDiscInfo.Layer1AdditionalMould ?? "";

            L2MasteringRing.Text = SubmissionInfo.CommonDiscInfo.Layer2MasteringRing ?? "";
            L2MasteringSID.Text = SubmissionInfo.CommonDiscInfo.Layer2MasteringSID ?? "";
            L2Toolstamp.Text = SubmissionInfo.CommonDiscInfo.Layer2ToolstampMasteringCode ?? ""; ;

            L3MasteringRing.Text = SubmissionInfo.CommonDiscInfo.Layer3MasteringRing ?? "";
            L3MasteringSID.Text = SubmissionInfo.CommonDiscInfo.Layer3MasteringSID ?? "";
            L3Toolstamp.Text = SubmissionInfo.CommonDiscInfo.Layer3ToolstampMasteringCode ?? "";

            Barcode.Text = SubmissionInfo.CommonDiscInfo.Barcode ?? "";
            Comments.Text = SubmissionInfo.CommonDiscInfo.Comments ?? "";
            Contents.Text = SubmissionInfo.CommonDiscInfo.Contents ?? "";

            Version.Text = SubmissionInfo.VersionAndEditions.Version ?? "";
            Edition.Text = SubmissionInfo.VersionAndEditions.OtherEditions ?? "";
        }

        /// <summary>
        /// Save the current contents of the UI to the base SubmissionInfo
        /// </summary>
        private void Save()
        {
            SubmissionInfo.CommonDiscInfo.Title = GameTitle.Text ?? "";
            SubmissionInfo.CommonDiscInfo.ForeignTitleNonLatin = ForeignTitle.Text ?? "";
            SubmissionInfo.CommonDiscInfo.DiscNumberLetter = DiscNumberLetter.Text ?? "";
            SubmissionInfo.CommonDiscInfo.DiscTitle = DiscTitle.Text ?? "";
            SubmissionInfo.CommonDiscInfo.Category = (CategoryComboBox.SelectedItem as Element<RedumpDiscCategory>)?.Value ?? RedumpDiscCategory.Games;
            SubmissionInfo.CommonDiscInfo.Region = (RegionComboBox.SelectedItem as Element<RedumpRegion>)?.Value ?? RedumpRegion.World;
            SubmissionInfo.CommonDiscInfo.Languages = Languages.Where(l => l.IsChecked).Select(l => l?.Value).ToArray();
            if (!SubmissionInfo.CommonDiscInfo.Languages.Any())
                SubmissionInfo.CommonDiscInfo.Languages = new RedumpLanguage?[] { null };
            SubmissionInfo.CommonDiscInfo.LanguageSelection = LanguageSelections.Where(ls => ls.IsChecked).Select(ls => ls?.Value).ToArray();
            SubmissionInfo.CommonDiscInfo.Serial = Serial.Text ?? "";
            SubmissionInfo.CommonDiscInfo.Layer0MasteringRing = L0MasteringRing.Text ?? "";
            SubmissionInfo.CommonDiscInfo.Layer0MasteringSID = L0MasteringSID.Text ?? "";
            SubmissionInfo.CommonDiscInfo.Layer0ToolstampMasteringCode = L0Toolstamp.Text ?? "";
            SubmissionInfo.CommonDiscInfo.Layer0MouldSID = L0MouldSID.Text ?? "";
            SubmissionInfo.CommonDiscInfo.Layer0AdditionalMould = L0AdditionalMould.Text ?? "";

            SubmissionInfo.CommonDiscInfo.Layer1MasteringRing = L1MasteringRing.Text ?? "";
            SubmissionInfo.CommonDiscInfo.Layer1MasteringSID = L1MasteringSID.Text ?? "";
            SubmissionInfo.CommonDiscInfo.Layer1ToolstampMasteringCode = L1Toolstamp.Text ?? "";
            SubmissionInfo.CommonDiscInfo.Layer1MouldSID = L1MouldSID.Text ?? "";
            SubmissionInfo.CommonDiscInfo.Layer1AdditionalMould = L1AdditionalMould.Text ?? "";

            SubmissionInfo.CommonDiscInfo.Layer2MasteringRing = L2MasteringRing.Text ?? "";
            SubmissionInfo.CommonDiscInfo.Layer2MasteringSID = L2MasteringSID.Text ?? "";
            SubmissionInfo.CommonDiscInfo.Layer2ToolstampMasteringCode = L2Toolstamp.Text ?? "";

            SubmissionInfo.CommonDiscInfo.Layer3MasteringRing = L3MasteringRing.Text ?? "";
            SubmissionInfo.CommonDiscInfo.Layer3MasteringSID = L3MasteringSID.Text ?? "";
            SubmissionInfo.CommonDiscInfo.Layer3ToolstampMasteringCode = L3Toolstamp.Text ?? "";

            SubmissionInfo.CommonDiscInfo.Barcode = Barcode.Text ?? "";
            SubmissionInfo.CommonDiscInfo.Comments = Comments.Text ?? "";
            SubmissionInfo.CommonDiscInfo.Contents = Contents.Text ?? "";

            SubmissionInfo.VersionAndEditions.Version = this.Version.Text ?? "";
            SubmissionInfo.VersionAndEditions.OtherEditions = Edition.Text ?? "";
        }

        #region Event Handlers

        /// <summary>
        /// Handler for AcceptButton Click event
        /// </summary>
        private void OnAcceptClick(object sender, RoutedEventArgs e)
        {
            Save();
            Hide();
        }

        /// <summary>
        /// Handler for CancelButton Click event
        /// </summary>
        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        /// <summary>
        /// Handler for DiscInformationWindow Closed event
        /// </summary>
        private void OnClosed(object sender, EventArgs e)
        {
            Hide();
        }

        #endregion
    }
}
