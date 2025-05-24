namespace PlayerController
{
    public interface IJumpHandler
    {
        void HandleJump(bool jumpPressed, bool isGrounded);
    }
}