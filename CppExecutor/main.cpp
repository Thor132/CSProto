#include <iostream>
#include "StandardBlocks.h"
#include "Executor.h"

/* TODO:
 * Async verification with functors
 * Add deseralization from json - need master method in order to determine what specialized type to create
 * DataStorage singleton update - change to global?
 * Need a way that a block can stop script execution with pass/fail.
 * Abstract Logging to an interface
 * Does it need more precise timing?
 *
 * Blocks:
 * Cleanup
 * EndScript
 */

using namespace ScriptLibrary;

std::list<BlockBase*>* GenerateTestBlocks();
int main(int argc, const char *argv[])
{
    std::list<BlockBase*>* listGen = GenerateTestBlocks();

    Script* pScript = new Script("TestScript");
    pScript->AppendBlockList(*listGen);
    delete listGen;

    Executor* pExecutor = new Executor();
    bool result = pExecutor->RunScript(pScript);

    std::cout <<"Script result: " << (result ? "success" : "failure") << "." <<std::endl;
    system("pause");
    return 0;
}

// Sample execution
std::list<BlockBase*>* GenerateTestBlocks()
{
    std::list<BlockBase*>* script = new std::list<BlockBase*>();

    StandardBlocks::ForLoop* forBlock = new StandardBlocks::ForLoop();
    StandardBlocks::Log* logBlock1 = new StandardBlocks::Log();
    StandardBlocks::Wait* waitBlock = new StandardBlocks::Wait();
    
    forBlock->SetIterations(5);
    logBlock1->SetText("Log message");
    waitBlock->SetWaitTime(1.5);
   
    forBlock->AddChild(logBlock1);
    forBlock->AddChild(waitBlock);
    script->push_back(forBlock);

    return script;
}