﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Linq;

namespace DataDiscovery.Core
{
    public class FileBase: INotifyPropertyChanged
    {

        private string _fileName;
        private bool _isFirstElemntHeader;
        private int _elmentCounter;
        private int _analyzedElmentCounter;
        private int _elmentColumnsCounter;
        private List<Tuple<string,int>> _elmentColumnsNames;
        private string _lastError;

        #region Constructor

        protected FileBase(string name, bool header)
        {
            _fileName = name;
            _isFirstElemntHeader = header;
            _elmentCounter = 0;
            _analyzedElmentCounter = 0;
            _elmentColumnsCounter = 0;
            _elmentColumnsNames = new List<Tuple<string,int>>();
            _lastError = string.Empty;
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        #region Property

        public string FileName => _fileName;

        public string LastError
        {
            get
            {
                return _lastError;
            }

            protected set
            {
                if(_lastError == value) 
                    return;
                
                _lastError = value;
                NotifyPropertyChanged();
            }
        }

        protected bool IsFirstElemntHeader
        {
            get
            {
                return _isFirstElemntHeader;
            }

            set
            {
                if (_isFirstElemntHeader == value)
                    return;

                _isFirstElemntHeader = value;
                NotifyPropertyChanged();
            }
        }

        public int AnalyzedElmentsCounter
        {
            get
            {
                return _analyzedElmentCounter;
            }

            protected set
            {
                if (_analyzedElmentCounter == value)
                    return;
                
                _analyzedElmentCounter = value;

                if(ElmentCounter < AnalyzedElmentsCounter) ElmentCounter = AnalyzedElmentsCounter;

                NotifyPropertyChanged();

            }
        }

        public int ElmentCounter
        {
            get
            { 
                return _elmentCounter;
            }

            protected set
            {
                if (_elmentCounter == value)
                    return;

                _elmentCounter = value;
                NotifyPropertyChanged();
            }
        }

        public int ElmentColumnsCounter
        {
            get
            { 
                return _elmentColumnsCounter;
            }

            private set 
            {
                if (_elmentColumnsCounter == value)
                    return;

                _elmentColumnsCounter = value;
                NotifyPropertyChanged();
            }
        }


        public string[] Header =>(from h  in  _elmentColumnsNames
            select h.Item1).ToArray();

        public string ElemntNames
        {
            get
            { 
                string columnsNames = string.Empty;
                for (int i = 0; i < _elmentColumnsCounter; i++)
                {
                    columnsNames = columnsNames + $"Column {i + 1}:\t{new String(' ', _elmentColumnsNames[i].Item2)}{_elmentColumnsNames[i].Item1}{Environment.NewLine}"; 
                }
                return columnsNames;
            }
        }

        #endregion

        protected void ClearElemntColumnsNames()
        {
            if (_elmentColumnsNames.Count == 0)
                return;
            
            _elmentColumnsNames.Clear();
            NotifyPropertyChanged("Header");
            NotifyPropertyChanged("ElemntNames");
            ElmentColumnsCounter = 0;
        }

        protected void AddElemntColumnsName(string name, int level = 1)
        {
            AddColumnsName(new Tuple<string,int>(name, level));
        }

        private void AddColumnsName(Tuple<string, int> column)
        {
            if (_elmentColumnsNames.Contains(column))
                return;

            _elmentColumnsNames.Add(column);
            NotifyPropertyChanged("Header");
            NotifyPropertyChanged("ElemntNames");
            ElmentColumnsCounter++;
        }

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

