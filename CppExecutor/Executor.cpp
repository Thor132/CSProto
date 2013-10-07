#include "Executor.h"
#include "Logger.h"

using namespace ScriptLibrary;

bool Executor::RunScript(Script* pScript)
{
    m_finished = false;
    m_passed = true;

    Logger::Trace("Running script " + pScript->GetName() + ".");
    RunBlock(pScript->GetBlockList());

    return m_passed;
}

void Executor::RunBlock(const std::list<BlockBase*>* pBlockList, bool forceExecution)
{
    for(std::list<BlockBase*>::const_iterator iter = pBlockList->begin(); iter != pBlockList->end(); ++iter)
    {
        BlockBase* Block = (*iter);

        // Check if the script is already finished and execution is not forced
        bool BlockForceExecution = IsBlockRequired(Block);
        if(m_finished && !forceExecution && !BlockForceExecution)
        {
            continue;
        }

        Logger::Trace("Running block " + Block->GetName() + ".");
        Block->SetState(BlockRunning);
        double totalTime = 0.0;
        std::time_t lastTime = std::clock(); // TODO: c++ 11 chrono
        Block->Run();

        while(Block->GetState() == BlockRunning)
        {
            // Run children if the Block has any.
            const std::list<BlockBase*>* pChildren = Block->GetChildren();
            if(Block->RunChildren() && pChildren->size() > 0)
            {
                RunBlock(pChildren, BlockForceExecution);
            }

            // Break early if execution should be finished.
            if(m_finished && !forceExecution && !BlockForceExecution)
            {
                break;
            }

            // Tick calculates the time since last measurement to send to Verify
            double tickTime;
            Tick(lastTime, tickTime);
            totalTime += tickTime;
            
            // Verify and check for timeouts
            Block->Verify(tickTime, totalTime);
            if(totalTime >= Block->GetTimeout())
            {
                Block->SetStatus(Timeout);
                Block->SetState(BlockTimeout);
            }
        }

        Block->Finish();

        if(!m_finished && Block->GetStatus() >= Block->GetThreshold())
        {
            m_finished = true;
            m_passed = false;
        }
    }
}

void Executor::Tick(std::time_t& lastTime, double& tickTime)
{
    std::time_t newTime = std::clock();
    tickTime = ( newTime - lastTime ) / (double) CLOCKS_PER_SEC;
    lastTime = newTime;
}

bool Executor::IsBlockRequired(const BlockBase* pBlock)
{
    return false;
}