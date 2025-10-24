import type {
	AIQueryResult,
	AIRequestResponseInfo,
	ReasoningEffortLevel,
} from '../services/excel-analysis.service';

import AIRequestCost from './base/AIRequestCost';

interface AnalysisInputs {
	modelType: string;
	reasoningLevel: ReasoningEffortLevel;
	datasetName: string;
}

interface AIAnalysisResultProps {
	result: AIQueryResult;
	inputs: AnalysisInputs;
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

	const renderUserMessage = () => (
		<div className="mb-6 message-bubble">
			<div className="d-flex align-items-end justify-content-end">
				<div className="me-3 message-width">
					<div className="position-relative">
						<div className="p-4 text-white position-relative user-message-bubble">
							<div className="message-text">
								{result.userQuery}
							</div>
						</div>
					</div>
				</div>
				<div className="flex-shrink-0">
					<div className="rounded-circle d-flex align-items-center justify-content-center text-white fw-bold avatar-float user-avatar">
						YOU
					</div>
				</div>
			</div>
		</div>
	);

	const renderMessage = (message: AIRequestResponseInfo, index: number) => {
		const isSystemMessage = message.isSynthetic || message.request;
		const isAIResponse = !message.isSynthetic && message.response;

		return (
			<div key={index} className="mb-6 message-bubble">
				{/* System/Request Message */}
				{isSystemMessage && (
					<div className="d-flex align-items-end">
						<div className="flex-shrink-0 me-3">
							<div className="rounded-circle d-flex align-items-center justify-content-center text-white fw-bold avatar-float system-avatar">
								SYS
							</div>
						</div>
						<div className="message-width">
							<div className="position-relative">
								<div className="p-4 text-dark position-relative system-message-bubble">
									<div className="message-text">
										{message.isSynthetic
											? message.response
											: message.request}
									</div>
								</div>
							</div>
						</div>
					</div>
				)}

				{/* AI Response Message */}
				{isAIResponse && (
					<div className="d-flex align-items-end">
						<div className="flex-shrink-0 me-3">
							<div className="rounded-circle d-flex align-items-center justify-content-center text-white fw-bold position-relative avatar-float ai-avatar">
								AI
								<div className="position-absolute rounded-circle avatar-online-indicator"></div>
							</div>
						</div>
						<div className="message-width">
							<div className="position-relative">
								<div className="p-4 text-white position-relative ai-message-bubble">
									<div className="message-text">
										{message.response}
									</div>
									<AIRequestCost cost={message.cost} />
								</div>
							</div>
						</div>
					</div>
				)}
			</div>
		);
	};

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
											<div className="chat-header-param">
												<span className="param-label">
													Dataset:
												</span>
												<span className="param-value">
													{inputs.datasetName}
												</span>
											</div>
											<div className="chat-header-param">
												<span className="param-label">
													Model:
												</span>
												<span className="param-value">
													{inputs.modelType}
												</span>
											</div>
											<div className="chat-header-param">
												<span className="param-label">
													Reasoning:
												</span>
												<span className="param-value">
													{inputs.reasoningLevel}
												</span>
											</div>
										</div>
									</div>

									{/* Divider */}
									<div className="chat-header-divider"></div>

									{/* Middle Section: Token & Cost Metrics */}
									<div className="chat-header-section">
										<div className="chat-header-label">
											AI Resource consumption
										</div>
										<div className="chat-header-metrics">
											<div className="chat-header-metric">
												<span className="metric-label">
													Input:
												</span>
												<span className="metric-value">
													{totals.inputTokens} tokens
												</span>
											</div>
											<div className="chat-header-metric">
												<span className="metric-label">
													Output:
												</span>
												<span className="metric-value">
													{totals.outputTokens} tokens
												</span>
											</div>
											<div className="chat-header-metric">
												<span className="metric-label">
													Reasoning:
												</span>
												<span className="metric-value">
													{totals.reasoningTokens}{' '}
													tokens
												</span>
											</div>
										</div>
									</div>

									<div className="chat-header-divider"></div>

									{/* Right Section: Costs */}
									<div className="chat-header-section">
										<div className="chat-header-label">
											Costs
										</div>
										<div className="chat-header-metrics">
											<div className="chat-header-metric cost-highlight">
												<span className="metric-label">
													Total:
												</span>
												<span className="metric-value">
													${totals.cost.toFixed(7)}
												</span>
											</div>
											<div className="chat-header-metric">
												<span className="metric-label">
													In 1$:
												</span>
												<span className="metric-value">
													{totals.cost > 0
														? (
																1 / totals.cost
														  ).toFixed(1)
														: '∞'}{' '}
													executions
												</span>
											</div>
										</div>
									</div>
								</div>
							</div>

							{/* Chat Container */}
							<div className="card-body p-4">
								<div className="chat-container pe-2">
									{/* User Query */}
									{renderUserMessage()}

									{/* AI Messages */}
									{result.requests.map((message, index) =>
										renderMessage(message, index)
									)}
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
