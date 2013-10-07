#pragma once

#include <list>

namespace ScriptLibrary
{
    enum BlockStatus
    {
        Success,
        Warning,
        Error,
        Fatal,
        Timeout
    };

    enum BlockState
    {
        BlockNotStarted,
        BlockRunning,
        BlockComplete,
        BlockTimeout
    };

    class BlockBase
    {
    private:
        std::string m_name;

    protected:
        BlockState m_state;
        BlockStatus m_status;  
        BlockStatus m_threshold;

        unsigned int m_timeout;
        std::list<BlockBase*> m_children;

    public:
        BlockBase(const char* name);
        ~BlockBase();
        virtual void Run() = 0;
        virtual void Verify(double tickTime, double totalTime);
        virtual void Finish();

        inline virtual bool RunChildren() { return false; }

        inline const std::string& GetName() { return m_name; }
        inline unsigned int GetTimeout() { return m_timeout; }
        inline const std::list<BlockBase*>* GetChildren() { return &m_children; }
        inline void AddChild(BlockBase* child) { m_children.push_back(child); }

        inline BlockState GetState() { return m_state; }
        inline BlockStatus GetStatus() { return m_status; }
        inline BlockStatus GetThreshold() { return m_threshold; }

        inline void SetState(BlockState state) { m_state = state; }
        inline void SetStatus(BlockStatus status) { m_status = status; }

    protected:
        virtual void SetComplete(BlockStatus status);
    };
}