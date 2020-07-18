using ImGuiNET;
using OpenTK;
using System;

namespace HaloMars.Assets
{
    /// <summary>An example game object with physical properties and a game loop.</summary>
    /// <seealso cref="HaloMars.GameObjectModel" />
    internal class Lander : GameObjectModel
    {
        private static bool _crashed;
        private static bool _landed;
        private float _enginePower;
        private float _workOfEngine;
        private float _velocity;
        private float _acceleration;
        private float _gravity;
        private float _mass;
        private float _maxImpactVelocity;
        private static bool start = false;
        private Random entrhopy = new Random();
        private PID _control;

        public Lander(Asset model, Vector4 position) : base(model, position)
        {
            _gravity = 5.0f;
            _mass = 1000f;
            _maxImpactVelocity = 25f;
            _velocity = 0f;
            _acceleration = 0f;
            _enginePower = 0f;
            _workOfEngine = 0f;
            _crashed = false;
            _landed = false;
            _control = new PID(1f, 0f, 0f, -_maxImpactVelocity, 10f);
        }

        public override void Update(FrameEventArgs e)
        {
            start = ImGui.Button("Rozpocznij / Wstrzymaj symulację") ? !start : start;
            if (ImGui.Button("Losuj parametry")) seed();
            ImGui.SameLine();
            if (ImGui.Button("Domyślne parametry")) benchmark();
            ImGui.Text("Masa : " + _mass.ToString() + " Przyspieszenie grawitacyjne : " + _gravity.ToString());
            ImGui.Text("Maksymalna dopuszczalna prędkość przyziemienia : " + _maxImpactVelocity.ToString());
            ImGui.Text("Prędkość : " + _velocity.ToString() + " Praca silników : " + _workOfEngine.ToString());
            if (ImGui.Button("Ustawienie początkowe")) zero();
            ImGui.Text("Wysokość : ");
            ImGui.ProgressBar((_position.Y + 1f) / 1.8f);

            _enginePower = _control.control(_velocity, (float)e.Time);
            if (start)
            {
                if (_position.Y <= -1f)
                {
                    _crashed = Math.Abs(_velocity) > _maxImpactVelocity ? true : false;
                    _landed = !_crashed;
                    ImGui.OpenPopup("Przyziemienie!");
                    ImGui.BeginPopupModal("Przyziemienie!");
                    ImGui.Text(_landed ?
                        "Gratulacje, udane lądowanie" :
                        "Niestety pojazd uległ uszkodzeniu, spróbuj zmniejszyć prędkość podejścia");
                    if (ImGui.Button("ok"))
                    {
                        ImGui.CloseCurrentPopup();
                        start = false;
                    }
                    ImGui.EndPopup();
                }
                else
                {
                    _acceleration = -_gravity + _enginePower;
                    _velocity += _acceleration * (float)e.Time;
                    _position.Y += _velocity * (float)e.Time * 0.01f;
                    _workOfEngine += Math.Abs(_enginePower * _mass * _velocity * (float)e.Time);
                }
            }
            //Plotting of controller 
            //ImGui.PlotLines()
        }

        public override void Render()
        {
            base.Render();
        }

        private void seed()
        {
            _mass = entrhopy.Next(750, 2500);
            _gravity = entrhopy.Next(10, 90) / 10f;
            _maxImpactVelocity = entrhopy.Next(100, 300) / 10f;
            _control.updateset(-_maxImpactVelocity);
        }

        private void benchmark()
        {
            _mass = 1000f;
            _gravity = 5f;
            _maxImpactVelocity = 25f;
            _control.updateset(-_maxImpactVelocity);
        }

        private void zero()
        {
            _position.Y = 0.9f;
            _velocity = 0f;
            _control.zero();
            _workOfEngine = 0f;
        }

        private class PID
        {
            private float _kp, _ki, _kd;
            private float _setpoint;
            private float _limit;
            private float _errorPrev = 0f;
            private float _errorInt = 0f;

            public PID(float kp, float ki, float kd, float sp, float max)
            {
                _kp = kp;
                _ki = ki;
                _kd = kd;
                _setpoint = sp;
                _limit = max;
            }

            public float control(float control, float dt)
            {
                ImGui.Text("Współczynnik proporcjonalny :");
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("Proporcja błędu sygnału w sygnale wyjściowym");
                    ImGui.EndTooltip();
                }
                ImGui.SliderFloat("Kp", ref _kp, 0f, 10f);
                ImGui.Text("Współczynnik całkujący");
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("Proporcja akumulaty błędu sygnału w sygnale wyjściowym");
                    ImGui.EndTooltip();
                }
                ImGui.SliderFloat("Ki", ref _ki, 0f, 10f);
                ImGui.Text("Współczynnik różniczkujący");
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("Proporcja pochodnej błędu sygnału w sygnale wyjściowym");
                    ImGui.EndTooltip();
                }
                ImGui.SliderFloat("Kd", ref _kd, 0f, 10f);
                float error = _setpoint - control;
                if (start & !(_crashed || _landed)) _errorInt += error * dt;
                ImGui.Text("Sygnał błędu : " + error.ToString() + " Suma sygnałów błędu : " + _errorInt.ToString());
                float derivative = (error - _errorPrev) / dt;
                ImGui.Text("Pochodna sygnału błędu : " + derivative.ToString());
                _errorPrev = error;
                float proportional = _kp * error;
                float integral = _ki * _errorInt;
                derivative = _kd * derivative;
                return (proportional + integral + derivative) > _limit ?
                    _limit :
                        ((proportional + integral + derivative) < 0 ?
                            0 :
                            (proportional + integral + derivative));
            }

            public void zero()
            {
                _errorInt = _errorPrev = 0f;
            }

            public void updateset(float sp)
            {
                _setpoint = sp;
            }
        }
    }
}