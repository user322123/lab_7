using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using lab7.Models;
using System.Text;
using ReactiveUI;
using System.IO;
using Avalonia.Controls;
using System.Text.RegularExpressions;

namespace lab7.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public MainWindowViewModel()
        {
            items = new ObservableCollection<Student>();
            averageStud = new ObservableCollection<Student>() { new Student("Сред. балл")};
        }

        ObservableCollection<Student> averageStud;
        public ObservableCollection<Student> AverageStud
        {
            get => averageStud;
            set => this.RaiseAndSetIfChanged(ref averageStud, value);
        }

        ObservableCollection<Student> items;
        public ObservableCollection<Student> Items
        {
            get => items;
            set => this.RaiseAndSetIfChanged(ref items, value);
        }

        public void AddStudent()
        {
            Items.Add(new Student("Ф.И.О."));
            CalckAverage();
        }

        public void DeleteChecked()
        {
            ObservableCollection<Student> nwItems = new ObservableCollection<Student>();
            foreach(var stud in items)
            {
                if(!stud.IsChecked)
                    nwItems.Add(stud);
            }
            Items = nwItems;
            CalckAverage();
        }

        public void SaveFile(string path)
        {
            using(StreamWriter sw = File.CreateText(path))
            {
                foreach(var stud in Items)
                {
                    sw.Write($"\"{stud.Name}\" ");
                    foreach(var grade in stud.Grades)
                    {
                        sw.Write($"{grade.Val} ");
                    }
                    sw.Write('\n');
                }
            }
        }

        public void OpenFile(string path)
        {

            if(File.Exists(path))
            {
                Items.Clear();
                Regex regex = new Regex("\\A\"[a-zA-Zа-яА-Я\\s]+\" \\d \\d \\d \\d \\z");
                using (StreamReader sr = File.OpenText(path))
                {
                    var str = "";
                    while((str = sr.ReadLine())!=null)
                    {
                        if(!regex.IsMatch(str))
                        {
                            return;
                        }
                        var index = str.IndexOf('"', 1);
                        var name = str.Substring(1, index-1);
                        var grades = str.Substring(index + 2).Split(' ');
                        var student = new Student(name);
                        for (int i = 0; i < 4; i++) student.Grades[i].Val = grades[i];
                        Items.Add(student);
                    }
                    CalckAverage();
                }
            }
        }
        public void CalckAverage()
        {
            List<float> grad = new List<float>() { 0, 0, 0, 0 };
            foreach(var stud in items)
            {
                for (int i = 0; i < stud.Grades.Count; i++) grad[i] += int.Parse(stud.Grades[i].Val);
            }
            for (int i = 0; i < 4; i++) {
                AverageStud[0].Grades[i].Val= (grad[i] / items.Count).ToString();
            }

        }

    }
}
