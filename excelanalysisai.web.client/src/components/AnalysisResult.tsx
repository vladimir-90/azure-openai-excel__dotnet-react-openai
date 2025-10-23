import type {
	AIQueryResult,
	AIRequestResponseInfo,
	OpenAIQueryCost,
} from '../services/excel-analysis.service';

interface AIAnalysisResultProps {
	result: AIQueryResult;
}

function AIAnalysisResult({ result }: AIAnalysisResultProps) {
	const formatCost = (cost: OpenAIQueryCost | null) => {
		if (!cost) return null;
		return (
			<div className="mt-2">
				<span className="badge rounded-pill px-3 py-2 cost-badge-ai">
					💰 ${cost.totalCost} • {cost.inputTokenCount}→
					<span>{cost.outputTokenCount} </span>
					{cost.reasoningTokenCount > 0 && (
						<span>({cost.reasoningTokenCount}) </span>
					)}
					<span>tokens</span>
				</span>
			</div>
		);
	};

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
									{message.cost && formatCost(message.cost)}
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
							<div className="card-header border-0 text-center py-4 chat-header">
								<h4 className="mb-1 fw-bold gradient-text">
									🤖 Excel Analysis AI
								</h4>
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
