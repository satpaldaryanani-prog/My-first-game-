// Cat Football Mobile - Touch Controls
const canvas = document.getElementById('gameCanvas');
const ctx = canvas.getContext('2d');

// Set canvas to full size
function resizeCanvas() {
    const container = canvas.parentElement;
    canvas.width = container.clientWidth;
    canvas.height = container.clientHeight;
}
resizeCanvas();
window.addEventListener('resize', resizeCanvas);

// Game Constants
const MATCH_DURATION = 180;
const BALL_RADIUS = 8;
const CAT_RADIUS = 15;
const BALL_MAX_SPEED = 12;
const CAT_BASE_SPEED = 3;
const CAT_SPRINT_MULTIPLIER = 1.8;
const STAMINA_MAX = 100;
const STAMINA_DRAIN_RATE = 15;
const STAMINA_REGEN_RATE = 10;
const BALL_FRICTION = 0.97;
const MEOWMENTUM_THRESHOLDS = [3, 6, 10, 15, 20];

// Game State
let gameStarted = false;
let gameState = {
    matchTime: MATCH_DURATION,
    homeScore: 0,
    awayScore: 0,
    matchInProgress: true,
    meowmentumChain: 0,
    meowmentumLevel: 0,
    lastActionTime: Date.now()
};

// Ball
let ball = {
    x: 0,
    y: 0,
    vx: 0,
    vy: 0,
    radius: BALL_RADIUS,
    possessed: false,
    possessor: null
};

// Player Cat
let player = {
    x: 0,
    y: 0,
    vx: 0,
    vy: 0,
    radius: CAT_RADIUS,
    stamina: STAMINA_MAX,
    speed: CAT_BASE_SPEED,
    name: "You",
    color: "#FF8C42",
    hasBall: false
};

// AI Cat
let ai = {
    x: 0,
    y: 0,
    vx: 0,
    vy: 0,
    radius: CAT_RADIUS,
    stamina: STAMINA_MAX,
    speed: CAT_BASE_SPEED * 0.8,
    name: "AI",
    color: "#6C5CE7",
    hasBall: false
};

// Input State
let joystickActive = false;
let joystickInput = { x: 0, y: 0 };
let isSprinting = false;

// Touch Controls
const joystickArea = document.getElementById('joystickArea');
const joystickStick = document.getElementById('joystickStick');
const sprintBtn = document.getElementById('sprintBtn');
const shootBtn = document.getElementById('shootBtn');
const powerBtn = document.getElementById('powerBtn');

// Joystick Setup
let joystickTouch = null;
let joystickCenter = { x: 60, y: 60 };
const joystickRadius = 35;

joystickArea.addEventListener('touchstart', handleJoystickStart, { passive: false });
joystickArea.addEventListener('touchmove', handleJoystickMove, { passive: false });
joystickArea.addEventListener('touchend', handleJoystickEnd, { passive: false });

function handleJoystickStart(e) {
    e.preventDefault();
    joystickTouch = e.touches[0].identifier;
    joystickActive = true;
    updateJoystick(e.touches[0]);
}

function handleJoystickMove(e) {
    e.preventDefault();
    if (!joystickActive) return;

    for (let touch of e.touches) {
        if (touch.identifier === joystickTouch) {
            updateJoystick(touch);
            break;
        }
    }
}

function handleJoystickEnd(e) {
    e.preventDefault();
    joystickActive = false;
    joystickInput = { x: 0, y: 0 };
    joystickStick.style.transform = 'translate(-50%, -50%)';
}

function updateJoystick(touch) {
    const rect = joystickArea.getBoundingClientRect();
    const x = touch.clientX - rect.left - joystickCenter.x;
    const y = touch.clientY - rect.top - joystickCenter.y;

    const distance = Math.sqrt(x * x + y * y);
    const angle = Math.atan2(y, x);

    const clampedDistance = Math.min(distance, joystickRadius);

    joystickInput.x = Math.cos(angle) * (clampedDistance / joystickRadius);
    joystickInput.y = Math.sin(angle) * (clampedDistance / joystickRadius);

    const stickX = Math.cos(angle) * clampedDistance;
    const stickY = Math.sin(angle) * clampedDistance;

    joystickStick.style.transform = `translate(calc(-50% + ${stickX}px), calc(-50% + ${stickY}px))`;
}

// Sprint Button
sprintBtn.addEventListener('touchstart', (e) => {
    e.preventDefault();
    isSprinting = true;
    sprintBtn.style.background = 'rgba(255, 152, 0, 0.9)';
});

sprintBtn.addEventListener('touchend', (e) => {
    e.preventDefault();
    isSprinting = false;
    sprintBtn.style.background = 'rgba(255, 193, 7, 0.9)';
});

// Action Buttons
shootBtn.addEventListener('touchstart', (e) => {
    e.preventDefault();
    if (player.hasBall) {
        shootBall(player, 1.0);
        addMeowmentumChain(2, "Shot!");
    }
});

powerBtn.addEventListener('touchstart', (e) => {
    e.preventDefault();
    if (player.hasBall) {
        shootBall(player, 1.5);
        addMeowmentumChain(2, "Power shot!");
    }
});

// Start Game
function startGame() {
    document.getElementById('tutorial').classList.remove('show');
    gameStarted = true;
    initGame();
    gameLoop();
}

function initGame() {
    ball.x = canvas.width / 2;
    ball.y = canvas.height / 2;
    player.x = canvas.width * 0.2;
    player.y = canvas.height / 2;
    ai.x = canvas.width * 0.8;
    ai.y = canvas.height / 2;

    showMessage("⚽ Match started! Score goals! 🐱");
}

// Game Loop
function gameLoop() {
    if (!gameStarted) return;

    update();
    render();
    requestAnimationFrame(gameLoop);
}

// Update
function update() {
    if (!gameState.matchInProgress) return;

    gameState.matchTime -= 1/60;
    if (gameState.matchTime <= 0) {
        gameState.matchTime = 0;
        endMatch();
    }

    updatePlayerInput();
    updateAI();
    updateBall();
    checkBallPossession();
    checkGoals();
    updateMeowmentum();
    updateUI();
}

function updatePlayerInput() {
    let moveX = joystickInput.x;
    let moveY = joystickInput.y;

    let currentSpeed = player.speed;

    if (isSprinting && player.stamina > 0) {
        currentSpeed *= CAT_SPRINT_MULTIPLIER;
        player.stamina -= STAMINA_DRAIN_RATE / 60;
        if (player.stamina < 0) player.stamina = 0;
    } else {
        player.stamina += STAMINA_REGEN_RATE / 60;
        if (player.stamina > STAMINA_MAX) player.stamina = STAMINA_MAX;
    }

    const meowmentumBonus = getMeowmentumBonus();
    currentSpeed *= (1 + meowmentumBonus);

    player.vx = moveX * currentSpeed;
    player.vy = moveY * currentSpeed;

    player.x += player.vx;
    player.y += player.vy;

    player.x = Math.max(player.radius, Math.min(canvas.width - player.radius, player.x));
    player.y = Math.max(player.radius, Math.min(canvas.height - player.radius, player.y));

    if (player.hasBall) {
        ball.x = player.x;
        ball.y = player.y;
    }
}

function updateAI() {
    const dx = ball.x - ai.x;
    const dy = ball.y - ai.y;
    const distance = Math.sqrt(dx * dx + dy * dy);

    if (distance > 5) {
        ai.vx = (dx / distance) * ai.speed;
        ai.vy = (dy / distance) * ai.speed;
    } else {
        ai.vx = 0;
        ai.vy = 0;
    }

    ai.x += ai.vx;
    ai.y += ai.vy;

    ai.x = Math.max(ai.radius, Math.min(canvas.width - ai.radius, ai.x));
    ai.y = Math.max(ai.radius, Math.min(canvas.height - ai.radius, ai.y));

    if (ai.hasBall && ai.x > canvas.width / 2) {
        shootBall(ai, 1.0);
    }

    if (ai.hasBall) {
        ball.x = ai.x;
        ball.y = ai.y;
    }
}

function updateBall() {
    if (ball.possessed) return;

    ball.vx *= BALL_FRICTION;
    ball.vy *= BALL_FRICTION;

    if (Math.abs(ball.vx) < 0.1) ball.vx = 0;
    if (Math.abs(ball.vy) < 0.1) ball.vy = 0;

    ball.x += ball.vx;
    ball.y += ball.vy;

    if (ball.x - ball.radius < 0 || ball.x + ball.radius > canvas.width) {
        ball.vx *= -0.8;
        ball.x = Math.max(ball.radius, Math.min(canvas.width - ball.radius, ball.x));
    }
    if (ball.y - ball.radius < 0 || ball.y + ball.radius > canvas.height) {
        ball.vy *= -0.8;
        ball.y = Math.max(ball.radius, Math.min(canvas.height - ball.radius, ball.y));
    }
}

function checkBallPossession() {
    const playerDist = Math.sqrt((player.x - ball.x) ** 2 + (player.y - ball.y) ** 2);
    if (playerDist < player.radius + ball.radius + 5 && !ball.possessed) {
        takeBall(player);
    }

    const aiDist = Math.sqrt((ai.x - ball.x) ** 2 + (ai.y - ball.y) ** 2);
    if (aiDist < ai.radius + ball.radius + 5 && !ball.possessed) {
        takeBall(ai);
    }
}

function takeBall(cat) {
    ball.possessed = true;
    ball.possessor = cat;
    cat.hasBall = true;
    ball.vx = 0;
    ball.vy = 0;

    if (cat === player) {
        addMeowmentumChain(1, "Ball!");
    } else {
        resetMeowmentumChain();
    }
}

function shootBall(cat, powerMultiplier) {
    if (!cat.hasBall) return;

    const targetX = cat === player ? canvas.width : 0;
    const targetY = canvas.height / 2 + (Math.random() - 0.5) * (canvas.height * 0.3);

    const dx = targetX - cat.x;
    const dy = targetY - cat.y;
    const distance = Math.sqrt(dx * dx + dy * dy);

    const power = 8 * powerMultiplier;
    ball.vx = (dx / distance) * power;
    ball.vy = (dy / distance) * power;

    ball.possessed = false;
    ball.possessor = null;
    cat.hasBall = false;
}

function checkGoals() {
    const goalWidth = 20;
    const goalHeight = canvas.height * 0.3;

    // Left goal (player defends)
    if (ball.x < goalWidth &&
        ball.y > canvas.height / 2 - goalHeight / 2 &&
        ball.y < canvas.height / 2 + goalHeight / 2) {
        gameState.awayScore++;
        showMessage("😿 AI Scored!");
        resetBall();
        resetMeowmentumChain();
    }

    // Right goal (player scores)
    if (ball.x > canvas.width - goalWidth &&
        ball.y > canvas.height / 2 - goalHeight / 2 &&
        ball.y < canvas.height / 2 + goalHeight / 2) {
        gameState.homeScore++;
        showMessage("🎉 GOAL!!!");
        addMeowmentumChain(5, "GOAL!");
        resetBall();
    }
}

function resetBall() {
    ball.x = canvas.width / 2;
    ball.y = canvas.height / 2;
    ball.vx = 0;
    ball.vy = 0;
    ball.possessed = false;
    ball.possessor = null;
    player.hasBall = false;
    ai.hasBall = false;

    player.x = canvas.width * 0.2;
    player.y = canvas.height / 2;
    ai.x = canvas.width * 0.8;
    ai.y = canvas.height / 2;
}

function updateMeowmentum() {
    const timeSinceAction = Date.now() - gameState.lastActionTime;
    if (timeSinceAction > 5000 && gameState.meowmentumChain > 0) {
        gameState.meowmentumChain -= 0.5 / 60;
        if (gameState.meowmentumChain < 0) gameState.meowmentumChain = 0;
    }

    let newLevel = 0;
    for (let i = 0; i < MEOWMENTUM_THRESHOLDS.length; i++) {
        if (gameState.meowmentumChain >= MEOWMENTUM_THRESHOLDS[i]) {
            newLevel = i + 1;
        }
    }

    if (newLevel !== gameState.meowmentumLevel) {
        gameState.meowmentumLevel = newLevel;
        if (newLevel > 0) {
            showMessage(`✨ Level ${newLevel}! +${getMeowmentumBonus() * 100}%`);
        }
    }
}

function addMeowmentumChain(points, action) {
    gameState.meowmentumChain += points;
    gameState.lastActionTime = Date.now();
}

function resetMeowmentumChain() {
    gameState.meowmentumChain = 0;
    gameState.meowmentumLevel = 0;
}

function getMeowmentumBonus() {
    const bonuses = [0, 0.05, 0.10, 0.15, 0.20, 0.25];
    return bonuses[gameState.meowmentumLevel] || 0;
}

function endMatch() {
    gameState.matchInProgress = false;

    let result = "";
    if (gameState.homeScore > gameState.awayScore) {
        result = "🎉 YOU WIN!";
    } else if (gameState.homeScore < gameState.awayScore) {
        result = "😿 YOU LOSE!";
    } else {
        result = "🤝 DRAW!";
    }

    showMessage(`⏱️ TIME'S UP! ${result}\nFinal: ${gameState.homeScore} - ${gameState.awayScore}\nTap to reload`);
}

function showMessage(message) {
    const msgEl = document.getElementById('message');
    msgEl.textContent = message;
    msgEl.classList.add('show');

    setTimeout(() => {
        msgEl.classList.remove('show');
    }, 2500);
}

function updateUI() {
    document.getElementById('score').textContent = `${gameState.homeScore} - ${gameState.awayScore}`;

    const minutes = Math.floor(gameState.matchTime / 60);
    const seconds = Math.floor(gameState.matchTime % 60);
    document.getElementById('time').textContent = `${minutes}:${seconds.toString().padStart(2, '0')}`;

    document.getElementById('chain').textContent = Math.floor(gameState.meowmentumChain);
    document.getElementById('bonus').textContent = `+${(getMeowmentumBonus() * 100).toFixed(0)}%`;

    const maxChain = MEOWMENTUM_THRESHOLDS[MEOWMENTUM_THRESHOLDS.length - 1];
    const percentage = Math.min(100, (gameState.meowmentumChain / maxChain) * 100);
    document.getElementById('meowFill').style.width = `${percentage}%`;
    document.getElementById('meowLevel').textContent = `Level ${gameState.meowmentumLevel}`;

    const staminaPercent = (player.stamina / STAMINA_MAX) * 100;
    document.getElementById('staminaFill').style.height = `${staminaPercent}%`;
}

// Render
function render() {
    ctx.fillStyle = '#88dd88';
    ctx.fillRect(0, 0, canvas.width, canvas.height);

    drawField();
    drawGoals();
    drawBall();
    drawCat(player);
    drawCat(ai);

    if (gameState.meowmentumLevel > 0) {
        drawMeowmentumEffect(player);
    }
}

function drawField() {
    ctx.strokeStyle = 'white';
    ctx.lineWidth = 2;

    ctx.beginPath();
    ctx.moveTo(canvas.width / 2, 0);
    ctx.lineTo(canvas.width / 2, canvas.height);
    ctx.stroke();

    ctx.beginPath();
    ctx.arc(canvas.width / 2, canvas.height / 2, Math.min(canvas.width, canvas.height) * 0.15, 0, Math.PI * 2);
    ctx.stroke();
}

function drawGoals() {
    const goalWidth = 20;
    const goalHeight = canvas.height * 0.3;

    ctx.fillStyle = 'rgba(255, 255, 255, 0.8)';
    ctx.fillRect(0, canvas.height / 2 - goalHeight / 2, goalWidth, goalHeight);
    ctx.fillRect(canvas.width - goalWidth, canvas.height / 2 - goalHeight / 2, goalWidth, goalHeight);
}

function drawBall() {
    ctx.fillStyle = 'white';
    ctx.strokeStyle = '#333';
    ctx.lineWidth = 2;
    ctx.beginPath();
    ctx.arc(ball.x, ball.y, ball.radius, 0, Math.PI * 2);
    ctx.fill();
    ctx.stroke();
}

function drawCat(cat) {
    ctx.fillStyle = cat.color;
    ctx.strokeStyle = '#333';
    ctx.lineWidth = 2;
    ctx.beginPath();
    ctx.arc(cat.x, cat.y, cat.radius, 0, Math.PI * 2);
    ctx.fill();
    ctx.stroke();

    ctx.fillStyle = 'white';
    ctx.beginPath();
    ctx.arc(cat.x, cat.y + 3, cat.radius * 0.6, 0, Math.PI * 2);
    ctx.fill();

    ctx.fillStyle = '#333';
    ctx.beginPath();
    ctx.arc(cat.x - 5, cat.y, 3, 0, Math.PI * 2);
    ctx.fill();
    ctx.beginPath();
    ctx.arc(cat.x + 5, cat.y, 3, 0, Math.PI * 2);
    ctx.fill();

    ctx.fillStyle = cat.color;
    ctx.beginPath();
    ctx.moveTo(cat.x - 8, cat.y - cat.radius);
    ctx.lineTo(cat.x - 12, cat.y - cat.radius - 8);
    ctx.lineTo(cat.x - 4, cat.y - cat.radius + 2);
    ctx.fill();
    ctx.stroke();
    ctx.beginPath();
    ctx.moveTo(cat.x + 8, cat.y - cat.radius);
    ctx.lineTo(cat.x + 12, cat.y - cat.radius - 8);
    ctx.lineTo(cat.x + 4, cat.y - cat.radius + 2);
    ctx.fill();
    ctx.stroke();
}

function drawMeowmentumEffect(cat) {
    const time = Date.now() * 0.005;
    const levels = [
        { color: 'rgba(102, 126, 234, 0.3)', size: 1.2 },
        { color: 'rgba(102, 126, 234, 0.4)', size: 1.4 },
        { color: 'rgba(118, 75, 162, 0.5)', size: 1.6 },
        { color: 'rgba(255, 215, 0, 0.6)', size: 1.8 },
        { color: 'rgba(255, 105, 180, 0.7)', size: 2.0 }
    ];

    const level = levels[gameState.meowmentumLevel - 1];
    if (level) {
        ctx.strokeStyle = level.color;
        ctx.lineWidth = 3;
        ctx.beginPath();
        ctx.arc(cat.x, cat.y, cat.radius * level.size * (1 + Math.sin(time) * 0.1), 0, Math.PI * 2);
        ctx.stroke();
    }
}
