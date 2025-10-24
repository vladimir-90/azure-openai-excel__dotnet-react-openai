import type {
	AIQueryResult,
	ReasoningEffortLevel,
} from '../services/excel-analysis.service';

import ChatMessage from './ChatMessage';

interface AIAnalysisResultProps {
	result: AIQueryResult;
	inputs: {
		modelType: string;
		reasoningLevel: ReasoningEffortLevel;
		datasetName: string;
	};
}

function AIAnalysisResult({ result, inputs }: AIAnalysisResultProps) {
	const totals = result.requests.reduce(
		(acc, msg) => {
			if (msg.cost) {
				return {
					cost: acc.cost + msg.cost.totalCost,
					inputTokens: acc.inputTokens + msg.cost.inputTokenCount,
					outputTokens: acc.outputTokens + msg.cost.outputTokenCount,
					reasoningTokens:
						acc.reasoningTokens + msg.cost.reasoningTokenCount,
				};
			}
			return acc;
		},
		{
			cost: 0,
			inputTokens: 0,
			outputTokens: 0,
			reasoningTokens: 0,
		}
	);

	// renderHeaderValue
	const rvh = (label: string, value: string | number) => (
		<div className="chat-header-param">
			<span className="param-label">{label}</span>
			<span className="param-value">{value}</span>
		</div>
	);

	return (
		<div className="chat-background">
			<div className="container py-4">
				<div className="row justify-content-center">
					<div className="col-12">
						<div className="card border-0 position-relative overflow-hidden chat-card">
							{/* Header */}
							<div className="card-header border-0 chat-header-professional">
								{/* Header Content */}
								<div className="chat-header-content">
									{/* Left Section: Query Parameters */}
									<div className="chat-header-section">
										<div className="chat-header-label">
											Query
										</div>
										<div className="chat-header-params">
											{rvh('Dataset', inputs.datasetName)}
											{rvh('Model', inputs.modelType)}
											{rvh(
												'Reasoning',
												inputs.reasoningLevel
											)}
										</div>
									</div>

									{/* Divider */}
									<div className="chat-header-divider"></div>

									{/* Middle Section: Token & Cost Metrics */}
									<div className="chat-header-section">
										<div className="chat-header-label">
											AI Resource consumption
										</div>
										<div className="chat-header-params">
											{rvh(
												'Input',
												`${totals.inputTokens} tokens`
											)}
											{rvh(
												'Output',
												`${totals.outputTokens} tokens`
											)}
											{rvh(
												'Reasoning',
												`${totals.reasoningTokens} tokens`
											)}
										</div>
									</div>

									<div className="chat-header-divider"></div>

									{/* Right Section: Costs */}
									<div className="chat-header-section">
										<div className="chat-header-label">
											Costs
										</div>
										<div className="chat-header-params">
											{rvh(
												'Total',
												`$ ${totals.cost.toFixed(7)}`
											)}
											{rvh(
												'In 1$',
												`${
													totals.cost > 0
														? (
																1 / totals.cost
														  ).toFixed(1)
														: '∞'
												} executions`
											)}
										</div>
									</div>
								</div>
							</div>

							{/* Chat Container */}
							<div className="card-body p-3">
								<div className="chat-container pe-2">
									{/* User Query */}
									<ChatMessage
										sender="you"
										text={result.userQuery}
									/>

									{/* AI Messages */}
									{result.requests.map((message, index) => (
										<>
											{!!message.request && (
												<ChatMessage
													key={`${index}-request`}
													sender="sys"
													text={message.request}
												/>
											)}

											{message.isSynthetic ? (
												<ChatMessage
													key={`${index}-response`}
													sender="sys"
													text={message.response}
												/>
											) : (
												<ChatMessage
													key={`${index}-response`}
													sender="ai"
													text={message.response}
													cost={message.cost}
												/>
											)}
										</>
									))}
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	);
}

export default AIAnalysisResult;
