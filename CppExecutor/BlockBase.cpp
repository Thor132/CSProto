#include "BlockBase.h"

using namespace ScriptLibrary;

BlockBase::BlockBase(const char* name)
    : m_name(name),
      m_state(BlockNotStarted),
      m_status(Success),
      m_threshold(Error)
{
}

BlockBase::~BlockBase()
{
    for(std::list<BlockBase*>::iterator iter = m_children.begin(); iter != m_children.end(); ++iter)
    {
        delete (*iter);
    }
}

void BlockBase::Verify(double tickTime, double totalTime)
{
    SetComplete(Success);
}

void BlockBase::Finish()
{
}

void BlockBase::SetComplete(BlockStatus status)
{
    m_status = status;
    m_state = BlockComplete;
}