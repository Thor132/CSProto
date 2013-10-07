#pragma once

#include "BlockBase.h"

namespace ScriptLibrary
{
    namespace StandardBlocks
    {
        // ForLoop Block
        class ForLoop : public BlockBase
        {
        private:
            int m_iterations;
            int m_currentIteration;

        public: 
            ForLoop();

            virtual void Run();
            virtual void Verify(double tickTime, double totalTime);

            inline void SetIterations(int iterations) { m_iterations = iterations; }
            inline virtual bool RunChildren() { return m_currentIteration < m_iterations; }
        }; 

        // Log Block
        // TODO: support different log levels
        class Log : public BlockBase
        {
        private:
            std::string m_message;

        public: 
            Log();

            virtual void Run();

            inline void SetText(const char* message) { m_message = message; }
        };

        // Wait Block
        class Wait : public BlockBase
        {
        private:
            double m_waitTime;

        public: 
            Wait();

            virtual void Run();
            virtual void Verify(double tickTime, double totalTime);

            inline void SetWaitTime(double time) { m_waitTime = time; }
        };
    }
}