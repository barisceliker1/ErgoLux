﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErgoLux;

partial class FrmMain
{
    /// <summary>
    /// Saves data into an elux format file.
    /// </summary>
    /// <param name="FileName">Path (including name) of the elux file</param>
    private void SaveELuxData(string FileName)
    {
        var cursor = Cursor.Current;
        Cursor.Current = Cursors.WaitCursor;

        try
        {
            using var fs = File.Open(FileName, FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite);
            using var sw = new StreamWriter(fs, Encoding.UTF8, leaveOpen: false);

            // Append millisecond pattern to current culture's full date time pattern
            string fullPattern = System.Globalization.DateTimeFormatInfo.CurrentInfo.FullDateTimePattern;
            fullPattern = System.Text.RegularExpressions.Regex.Replace(fullPattern, "(:ss|:s)", _sett.MillisecondsFormat);
            TimeSpan nTime = _timeEnd - _timeStart;

            // Save the header text into the file
            sw.WriteLine("ErgoLux data");
            sw.WriteLine("Start time: {0}", _timeStart.ToString(fullPattern));
            sw.WriteLine("End time: {0}", _timeEnd.ToString(fullPattern));
            //outfile.WriteLine("Total measuring time: {0} days, {1} hours, {2} minutes, {3} seconds, and {4} milliseconds ({5})", nTime.Days, nTime.Hours, nTime.Minutes, nTime.Seconds, nTime.Milliseconds, nTime.ToString(@"dd\-hh\:mm\:ss.fff"));
            sw.WriteLine("Total measuring time: {0} days, {1} hours, {2} minutes, {3} seconds, and {4} milliseconds", nTime.Days, nTime.Hours, nTime.Minutes, nTime.Seconds, nTime.Milliseconds);
            sw.WriteLine("Number of sensors: {0}", _sett.T10_NumberOfSensors.ToString());
            sw.WriteLine("Number of data points: {0}", _nPoints.ToString());
            sw.WriteLine("Sampling frequency: {0}", _sett.T10_Frequency.ToString());
            sw.WriteLine();
            string content = string.Empty;
            for (int i = 0; i < _sett.T10_NumberOfSensors; i++)
                content += "Sensor #" + i.ToString("00") + "\t";
            content += "Maximum" + "\t" + "Average" + "\t" + "Minimum" + "\t" + "Min/Average" + "\t" + "Min/Max" + "\t" + "Average/Max";
            sw.WriteLine(content);

            // Save the numerical values
            for (int j = 0; j < _nPoints; j++)
            {
                content = string.Empty;
                for (int i = 0; i < _plotData.Length; i++)
                {
                    content += _plotData[i][j].ToString(_sett.DataFormat) + "\t";
                }
                //trying to write data to csv
                sw.WriteLine(content.TrimEnd('\t'));
            }

            // Show OK save data
            using (new CenterWinDialog(this))
            {
                MessageBox.Show(StringsRM.GetString("strMsgBoxSaveData", _sett.AppCulture) ?? "Data has been successfully saved to disk.",
                    StringsRM.GetString("strMsgBoxSaveDataTitle", _sett.AppCulture) ?? "Data saving",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
        catch
        {
            // Show error message
            using (new CenterWinDialog(this))
            {
                MessageBox.Show(StringsRM.GetString("strMsgBoxErrorSaveData", _sett.AppCulture) ?? "An unexpected error happened while saving data to disk.\nPlease try again later or contact the software engineer.",
                    StringsRM.GetString("strMsgBoxErrorSaveDataTitle", _sett.AppCulture) ?? "Error saving data",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        finally
        {
            Cursor.Current = cursor;
        }
    }

    /// <summary>
    /// Saves data into a text format file.
    /// </summary>
    /// <param name="FileName">Path (including name) of the elux file</param>
    private void SaveTextData (string FileName)
    {
        throw new Exception("Saving to text has not yet been implemented.");
    }

    /// <summary>
    /// Saves data into a binary format file.
    /// </summary>
    /// <param name="FileName">Path (including name) of the elux file</param>
    private void SaveBinaryData (string FileName)
    {
        try
        {
            using var fs = File.Open(FileName, FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite);
            using var bw = new BinaryWriter(fs, Encoding.UTF8, false);

            // Append millisecond pattern to current culture's full date time pattern
            string fullPattern = System.Globalization.DateTimeFormatInfo.CurrentInfo.FullDateTimePattern;
            fullPattern = System.Text.RegularExpressions.Regex.Replace(fullPattern, "(:ss|:s)", _sett.MillisecondsFormat);
            TimeSpan nTime = _timeEnd - _timeStart;

            // Save the header text into the file
            bw.WriteLine("ErgoLux data");
            bw.WriteLine($"Start time: {_timeStart.ToString(fullPattern)}");
            bw.WriteLine($"End time: {_timeEnd.ToString(fullPattern)}");
            bw.WriteLine($"Total measuring time: {nTime.Days} days, {nTime.Hours} hours, {nTime.Minutes} minutes, {nTime.Seconds} seconds, and {nTime.Milliseconds} milliseconds");
            bw.WriteLine($"Number of sensors: {_sett.T10_NumberOfSensors}");
            bw.WriteLine($"Number of data points: {_nPoints}");
            bw.WriteLine($"Sampling frequency: {_sett.T10_Frequency}");
            bw.WriteLine();
            string content = string.Empty;
            for (int i = 0; i < _sett.T10_NumberOfSensors; i++)
                content += "Sensor #" + i.ToString("00") + "\t";
            content += "Maximum" + "\t" + "Average" + "\t" + "Minimum" + "\t" + "Min/Average" + "\t" + "Min/Max" + "\t" + "Average/Max";
            bw.WriteLine(content);

            // Save the numerical values
            for (int j = 0; j < _nPoints; j++)
            {
                content = string.Empty;
                for (int i = 0; i < _plotData.Length; i++)
                {
                    content += _plotData[i][j].ToString(_sett.DataFormat) + "\t";
                }
                //trying to write data to csv
                bw.WriteLine(content.TrimEnd('\t'));
            }

            // Show OK save data
            using (new CenterWinDialog(this))
            {
                MessageBox.Show(StringsRM.GetString("strMsgBoxSaveData", _sett.AppCulture) ?? "Data has been successfully saved to disk.",
                    StringsRM.GetString("strMsgBoxSaveDataTitle", _sett.AppCulture) ?? "Data saving",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
        catch
        {
            // Show error message
            using (new CenterWinDialog(this))
            {
                MessageBox.Show(StringsRM.GetString("strMsgBoxErrorSaveData", _sett.AppCulture) ?? "An unexpected error happened while saving data to disk.\nPlease try again later or contact the software engineer.",
                    StringsRM.GetString("strMsgBoxErrorSaveDataTitle", _sett.AppCulture) ?? "Error saving data",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }

    /// <summary>
    /// Saves data. Default behaviour
    /// </summary>
    /// <param name="FileName">Path (including name) of the elux file</param>
    private void SaveDefaultData(string FileName)
    {
        using (new CenterWinDialog(this))
        {
            MessageBox.Show("No data has been saved to disk.", "No data saved", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

}

