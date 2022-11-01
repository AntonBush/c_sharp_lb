﻿/*

Задание: 
Разработать программу, реализующую работу с файлами.
1. Программа должна быть разработана в виде приложения Windows 
Forms на языке C#. По желанию вместо Windows Forms возможно 
использование WPF (Windows Presentation Foundation).
2. Добавить кнопку, реализующую функцию чтения текстового файла в 
список слов List<string>.
3. Для выбора имени файла используется класс OpenFileDialog, который 
открывает диалоговое окно с выбором файла. Ограничить выбор только 
файлами с расширением «.txt».
4. Для чтения из файла рекомендуется использовать статический метод 
ReadAllText() класса File (пространство имен System.IO). Содержимое файла 
считывается методом ReadAllText() в виде одной строки, далее делится на 
слова с использованием метода Split() класса string. Слова сохраняются в 
список List<string>.
5. При сохранении слов в список List<string> дубликаты слов не 
записываются. Для проверки наличия слова в списке используется метод 
Contains().
6. Вычислить время загрузки и сохранения в список с использованием 
класса Stopwatch (пространство имен System.Diagnostics). Вычисленное время 
вывести на форму в поле ввода (TextBox) или надпись (Label).
7. Добавить на форму поле ввода для поиска слова и кнопку поиска. При 
нажатии на кнопку поиска осуществлять поиск введенного слова в списке 
Слово считается найденным, если оно входит в элемент списка как подстрока 
(метод Contains() класса string).
8. Добавить на форму список (ListBox). Найденные слова выводить в 
список с использованием метода «название_списка.Items.Add()». Вызовы 
метода «название_списка.Items.Add()» должны находится между вызовами 
методов «название_списка.BeginUpdate()» и «название_списка. EndUpdate()».
9. Вычислить время поиска с использованием класса Stopwatch. 
Вычисленное время вывести на форму в поле ввода (TextBox) или надпись 
(Label).

*/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace lb5
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
    }
}
